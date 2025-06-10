using Microsoft.AspNetCore.Mvc;
using ZapPay.Application.DTOs.Auth;
using ZapPay.Application.Interfaces;

namespace ZapPay.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IOtpService _otpService;

    public AuthController(IOtpService otpService)
    {
        _otpService = otpService;
    }

    [HttpPost("send-otp")]
    public async Task<IActionResult> SendOtp([FromBody] SendOtpRequestDto request)
    {
        var (success, message) = await _otpService.SendOtpAsync(request);
        if (!success)
            return BadRequest(new { message });

        return Ok(new { message });
    }

    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequestDto request)
    {
        var (success, message, token, user) = await _otpService.VerifyOtpAsync(request);
        if (!success)
            return BadRequest(new { message });

        return Ok(new { message, token, user });
    }
} 