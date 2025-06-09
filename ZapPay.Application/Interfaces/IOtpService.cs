using ZapPay.Application.DTOs.Auth;

namespace ZapPay.Application.Interfaces;

public interface IOtpService
{
    Task<(bool success, string message)> SendOtpAsync(SendOtpRequestDto request);
    Task<(bool success, string message, string? token)> VerifyOtpAsync(VerifyOtpRequestDto request);
} 