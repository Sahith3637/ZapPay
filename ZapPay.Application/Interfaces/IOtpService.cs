using ZapPay.Application.DTOs.Auth;
using ZapPay.Application.DTOs;

namespace ZapPay.Application.Interfaces;

public interface IOtpService
{
    Task<(bool success, string message)> SendOtpAsync(SendOtpRequestDto request);
    Task<(bool success, string message, string? token, RegisterUserResponseDto? user)> VerifyOtpAsync(VerifyOtpRequestDto request);
} 