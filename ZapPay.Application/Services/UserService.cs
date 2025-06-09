using AutoMapper;
using ZapPay.Application.DTOs;
using ZapPay.Application.Interfaces;
using ZapPay.Domain.Entities;
using ZapPay.Infrastructure.Interfaces;
using ZapPay.Persistence.Interfaces;
using BCrypt.Net;
using Serilog;

namespace ZapPay.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IKycRepository _kycRepository;
    private readonly IAuditLogService _auditLogService;
    private readonly IMapper _mapper;
    private readonly Guid _systemUserId = Guid.Parse("00000000-0000-0000-0000-000000000001");
    private readonly IVpaGeneratorService _vpaGeneratorService;
    private readonly IVpaRepository _vpaRepository;

    public UserService(
        IUserRepository userRepository,
        IKycRepository kycRepository,
        IAuditLogService auditLogService,
        IMapper mapper,
        IVpaGeneratorService vpaGeneratorService,
        IVpaRepository vpaRepository)
    {
        _userRepository = userRepository;
        _kycRepository = kycRepository;
        _auditLogService = auditLogService;
        _mapper = mapper;
        _vpaGeneratorService = vpaGeneratorService;
        _vpaRepository = vpaRepository;
    }

    public async Task<RegisterUserResponseDto> RegisterUserAsync(RegisterUserRequestDto request)
    {
        // Check if mobile number or email already exists
        if (await _userRepository.ExistsByMobileNumberAsync(request.MobileNumber))
            throw new InvalidOperationException("Mobile number already registered");

        if (await _userRepository.ExistsByEmailAsync(request.Email))
            throw new InvalidOperationException("Email already registered");

        // Create user with pending KYC status
        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            MobileNumber = request.MobileNumber,
            Email = request.Email,
            DateOfBirth = request.DateOfBirth,
            Status = "Pending", // User status is pending until KYC is verified
            Kycstatus = "Pending", // KYC status is pending
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = _systemUserId,
            UpdatedAt = DateTime.UtcNow,
            UpdatedBy = _systemUserId
        };

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        try
        {
            Log.Information($"Attempting to generate VPA for user {user.UserId} ({user.MobileNumber})");
            var vpaValue = await _vpaGeneratorService.GenerateUniqueVpaAsync(user.MobileNumber);
            Log.Information($"Generated VPA: {vpaValue}");
            var vpa = new VirtualPaymentAddress {
                UserId = user.UserId,
                Vpa = vpaValue,
                CreatedAt = DateTime.UtcNow,
                Status = "Active",
                CreatedBy = _systemUserId,
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = _systemUserId
            };
            await _vpaRepository.AddVpaAsync(vpa);
            Log.Information($"Saved VPA to database for user {user.UserId}");
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Failed to create VPA for user {user.UserId}");
            throw;
        }

        // Reload user with VPA navigation property
        var userWithVpa = await _userRepository.GetByIdAsync(user.UserId, includeVpa: true);

        // Log the registration
        await _auditLogService.LogAsync(
            user.UserId,
            "Register",
            "User",
            user.UserId,
            null,
            $"User registered with mobile: {user.MobileNumber}, VPA: {userWithVpa?.VirtualPaymentAddresses?.FirstOrDefault()?.Vpa}");

        return _mapper.Map<RegisterUserResponseDto>(userWithVpa);
    }

    public async Task<RegisterUserResponseDto> VerifyKycAsync(Guid userId, string verificationStatus, string? verificationRemarks)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new KeyNotFoundException("User not found");

        var kyc = (await _kycRepository.FindAsync(k => k.UserId == userId && !k.IsDeleted)).FirstOrDefault();
        if (kyc == null)
            throw new KeyNotFoundException("KYC record not found");

        var oldStatus = kyc.VerificationStatus;
        kyc.VerificationStatus = verificationStatus;
        kyc.VerificationRemarks = verificationRemarks;
        kyc.UpdatedAt = DateTime.UtcNow;
        kyc.UpdatedBy = _systemUserId;

        if (verificationStatus == "Verified")
        {
            user.Status = "Active";
            user.Kycstatus = "Verified";
        }
        else if (verificationStatus == "Rejected")
        {
            user.Status = "Inactive";
            user.Kycstatus = "Rejected";
            kyc.RetryCount++;
        }

        user.UpdatedAt = DateTime.UtcNow;
        user.UpdatedBy = _systemUserId;

        await _kycRepository.UpdateAsync(kyc);
        await _userRepository.UpdateAsync(user);
        await _kycRepository.SaveChangesAsync();
        await _userRepository.SaveChangesAsync();

        // Log the KYC verification
        await _auditLogService.LogAsync(
            userId,
            "VerifyKYC",
            "UserKYC",
            kyc.Kycid,
            oldStatus,
            verificationStatus);

        return _mapper.Map<RegisterUserResponseDto>(user);
    }

    public async Task<RegisterUserResponseDto> ResubmitKycAsync(Guid userId, ResubmitKycRequestDto request)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new KeyNotFoundException("User not found");

        var kyc = (await _kycRepository.FindAsync(k => k.UserId == userId && !k.IsDeleted)).FirstOrDefault();
        if (kyc == null)
            throw new KeyNotFoundException("KYC record not found");

        if (kyc.RetryCount >= kyc.MaxRetries)
            throw new InvalidOperationException("Maximum retry attempts exceeded");

        var oldDocumentNumber = kyc.DocumentNumber;
        var hashedDocumentNumber = BCrypt.Net.BCrypt.HashPassword(request.DocumentNumber);
        kyc.DocumentNumber = hashedDocumentNumber;
        kyc.DocumentHash = hashedDocumentNumber;
        kyc.VerificationStatus = "Pending";
        kyc.VerificationRemarks = null;
        kyc.UpdatedAt = DateTime.UtcNow;
        kyc.UpdatedBy = _systemUserId;

        user.Status = "Pending";
        user.Kycstatus = "Pending";
        user.UpdatedAt = DateTime.UtcNow;
        user.UpdatedBy = _systemUserId;

        await _kycRepository.UpdateAsync(kyc);
        await _userRepository.UpdateAsync(user);
        await _kycRepository.SaveChangesAsync();
        await _userRepository.SaveChangesAsync();

        // Log the KYC resubmission
        await _auditLogService.LogAsync(
            userId,
            "ResubmitKYC",
            "UserKYC",
            kyc.Kycid,
            oldDocumentNumber,
            hashedDocumentNumber);

        return _mapper.Map<RegisterUserResponseDto>(user);
    }

    public async Task AddKycWithFileAsync(Guid userId, AddKycWithFileRequestDto request, string filePath)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) throw new KeyNotFoundException("User not found");

        var hashedDocumentNumber = BCrypt.Net.BCrypt.HashPassword(request.DocumentNumber);

        var kyc = new UserKyc
        {
            UserId = userId,
            DocumentType = request.DocumentType,
            DocumentNumber = hashedDocumentNumber,
            DocumentHash = hashedDocumentNumber,
            DocumentFilePath = filePath,
            VerificationStatus = "Pending",
            RetryCount = 0,
            MaxRetries = 3,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = _systemUserId,
            UpdatedAt = DateTime.UtcNow,
            UpdatedBy = _systemUserId
        };

        await _kycRepository.AddAsync(kyc);
        await _kycRepository.SaveChangesAsync();
    }

    public async Task<UserKyc?> GetKycByIdAsync(Guid kycId)
    {
        return (await _kycRepository.FindAsync(k => k.Kycid == kycId && !k.IsDeleted)).FirstOrDefault();
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _userRepository.FindAsync(u => !u.IsDeleted);
    }

    public async Task<IEnumerable<UserKyc>> GetAllKycAsync()
    {
        return await _kycRepository.FindAsync(k => !k.IsDeleted);
    }

    public async Task<User> UpdateProfileAsync(Guid userId, UpdateProfileDto dto)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new KeyNotFoundException("User not found");

        var oldUser = new User
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };

        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.Email = dto.Email;
        user.UpdatedAt = DateTime.UtcNow;
        user.UpdatedBy = _systemUserId;

        await _userRepository.UpdateAsync(user);
        await _userRepository.SaveChangesAsync();

        // Audit log: log old vs new data
        await _auditLogService.LogAsync(
            userId,
            "UpdateProfile",
            "User",
            user.UserId,
            $"FirstName: {oldUser.FirstName}, LastName: {oldUser.LastName}, Email: {oldUser.Email}",
            $"FirstName: {user.FirstName}, LastName: {user.LastName}, Email: {user.Email}");

        return user;
    }

    public async Task AssignVpaToUsersWithoutOneAsync()
    {
        var users = await _userRepository.FindAsync(u => !u.IsDeleted);
        foreach (var user in users)
        {
            var hasVpa = await _vpaRepository.GetByVpaAsync(user.MobileNumber + "@upi");
            if (hasVpa == null)
            {
                var vpaValue = await _vpaGeneratorService.GenerateUniqueVpaAsync(user.MobileNumber);
                var vpa = new VirtualPaymentAddress {
                    UserId = user.UserId,
                    Vpa = vpaValue,
                    CreatedAt = DateTime.UtcNow,
                    Status = "Active",
                    CreatedBy = _systemUserId,
                    UpdatedAt = DateTime.UtcNow,
                    UpdatedBy = _systemUserId
                };
                await _vpaRepository.AddVpaAsync(vpa);
                await _auditLogService.LogAsync(
                    user.UserId,
                    "AssignVPA",
                    "User",
                    user.UserId,
                    null,
                    $"Assigned VPA: {vpaValue}");
            }
        }
    }
} 