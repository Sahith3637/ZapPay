using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ZapPay.Application.DTOs.Auth;
using ZapPay.Application.Interfaces;
using ZapPay.Domain.Entities;
using ZapPay.Infrastructure.Configuration;
using ZapPay.Infrastructure.Interfaces;
using ZapPay.Persistence.Interfaces;

namespace ZapPay.Application.Services;

public class OtpService : IOtpService
{
    private readonly IUserRepository _userRepository;
    private readonly IGenericRepository<Otpverification> _otpRepository;
    private readonly IAuditLogService _auditLogService;
    private readonly IMemoryCache _cache;
    private readonly JwtSettings _jwtSettings;
    private readonly Guid _systemUserId = Guid.Parse("00000000-0000-0000-0000-000000000001");
    private const string RateLimitKeyPrefix = "OTP_RATE_LIMIT_";
    private const int MaxOtpAttempts = 3;
    private const int RateLimitWindowMinutes = 10;
    private const int OtpExpiryMinutes = 5;

    public OtpService(
        IUserRepository userRepository,
        IGenericRepository<Otpverification> otpRepository,
        IAuditLogService auditLogService,
        IMemoryCache cache,
        IOptions<JwtSettings> jwtSettings)
    {
        _userRepository = userRepository;
        _otpRepository = otpRepository;
        _auditLogService = auditLogService;
        _cache = cache;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<(bool success, string message)> SendOtpAsync(SendOtpRequestDto request)
    {
        // Check rate limiting
        var rateLimitKey = $"{RateLimitKeyPrefix}{request.MobileNumber}";
        if (_cache.TryGetValue(rateLimitKey, out int attempts) && attempts >= MaxOtpAttempts)
        {
            return (false, $"Too many OTP requests. Please try again after {RateLimitWindowMinutes} minutes.");
        }

        // Check if user exists
        var user = await _userRepository.GetByMobileNumberAsync(request.MobileNumber);
        if (user == null)
        {
            return (false, "User not found with this mobile number.");
        }

        // Generate OTP
        var otp = GenerateOtp();
        var expiryTime = DateTime.UtcNow.AddMinutes(OtpExpiryMinutes);

        // Save OTP
        var otpVerification = new Otpverification
        {
            UserId = user.UserId,
            Otptype = "Login",
            Otpvalue = otp,
            ContactType = "Mobile",
            ContactValue = request.MobileNumber,
            Status = "Pending",
            ExpiresAt = expiryTime,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = _systemUserId,
            UpdatedAt = DateTime.UtcNow,
            UpdatedBy = _systemUserId
        };

        await _otpRepository.AddAsync(otpVerification);
        await _otpRepository.SaveChangesAsync();

        // Update rate limit
        _cache.Set(rateLimitKey, (attempts + 1), TimeSpan.FromMinutes(RateLimitWindowMinutes));

        // Log OTP generation
        await _auditLogService.LogAsync(
            user.UserId,
            "SendOTP",
            "OTPVerification",
            otpVerification.Otpid,
            null,
            $"OTP sent to {request.MobileNumber}");

        // TODO: In production, integrate with SMS service
        Console.WriteLine($"OTP for {request.MobileNumber}: {otp}");

        return (true, "OTP sent successfully.");
    }

    public async Task<(bool success, string message, string? token)> VerifyOtpAsync(VerifyOtpRequestDto request)
    {
        // Get user
        var user = await _userRepository.GetByMobileNumberAsync(request.MobileNumber);
        if (user == null)
        {
            return (false, "User not found.", null);
        }

        // Get latest OTP
        var otpVerification = (await _otpRepository.FindAsync(o =>
            o.UserId == user.UserId &&
            o.ContactValue == request.MobileNumber &&
            o.Status == "Pending" &&
            o.ExpiresAt > DateTime.UtcNow))
            .OrderByDescending(o => o.CreatedAt)
            .FirstOrDefault();

        if (otpVerification == null)
        {
            return (false, "No valid OTP found. Please request a new OTP.", null);
        }

        if (otpVerification.Otpvalue != request.Otp)
        {
            // Log failed attempt
            await _auditLogService.LogAsync(
                user.UserId,
                "VerifyOTP",
                "OTPVerification",
                otpVerification.Otpid,
                "Failed",
                "Invalid OTP");

            return (false, "Invalid OTP.", null);
        }

        // Update OTP status
        otpVerification.Status = "Verified";
        otpVerification.VerifiedAt = DateTime.UtcNow;
        otpVerification.UpdatedAt = DateTime.UtcNow;
        otpVerification.UpdatedBy = _systemUserId;

        await _otpRepository.UpdateAsync(otpVerification);
        await _otpRepository.SaveChangesAsync();

        // Generate JWT token
        var token = GenerateJwtToken(user);

        // Log successful verification
        await _auditLogService.LogAsync(
            user.UserId,
            "VerifyOTP",
            "OTPVerification",
            otpVerification.Otpid,
            "Success",
            "OTP verified successfully");

        return (true, "OTP verified successfully.", token);
    }

    private string GenerateOtp()
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString();
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.MobilePhone, user.MobileNumber),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.Role, "User")
            }),
            Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpiryInHours),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
} 