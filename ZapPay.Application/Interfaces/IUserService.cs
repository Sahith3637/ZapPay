using ZapPay.Application.DTOs;
using ZapPay.Domain.Entities;

namespace ZapPay.Application.Interfaces;

public interface IUserService
{
    Task<RegisterUserResponseDto> RegisterUserAsync(RegisterUserRequestDto request);
    Task<RegisterUserResponseDto> VerifyKycAsync(Guid userId, string verificationStatus, string? verificationRemarks);
    Task<RegisterUserResponseDto> ResubmitKycAsync(Guid userId, ResubmitKycRequestDto request);
    Task AddKycWithFileAsync(Guid userId, AddKycWithFileRequestDto request, string filePath);
    Task<UserKyc?> GetKycByIdAsync(Guid kycId);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<IEnumerable<UserKyc>> GetAllKycAsync();
    Task<User> UpdateProfileAsync(Guid userId, UpdateProfileDto dto);
} 