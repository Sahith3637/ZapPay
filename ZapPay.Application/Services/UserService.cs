using AutoMapper;
using ZapPay.Application.DTOs;
using ZapPay.Application.Interfaces;
using ZapPay.Domain.Entities;
using ZapPay.Infrastructure.Interfaces;
using ZapPay.Persistence.Interfaces;
using BCrypt.Net;

namespace ZapPay.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IKycRepository _kycRepository;
    private readonly IAuditLogService _auditLogService;
    private readonly IMapper _mapper;
    private readonly Guid _systemUserId = Guid.Parse("00000000-0000-0000-0000-000000000001");

    public UserService(
        IUserRepository userRepository,
        IKycRepository kycRepository,
        IAuditLogService auditLogService,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _kycRepository = kycRepository;
        _auditLogService = auditLogService;
        _mapper = mapper;
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

        // Log the registration
        await _auditLogService.LogAsync(
            user.UserId,
            "Register",
            "User",
            user.UserId,
            null,
            $"User registered with mobile: {user.MobileNumber}");

        return _mapper.Map<RegisterUserResponseDto>(user);
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
} 