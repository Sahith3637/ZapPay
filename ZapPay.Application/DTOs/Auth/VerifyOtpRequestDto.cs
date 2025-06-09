using System.ComponentModel.DataAnnotations;

namespace ZapPay.Application.DTOs.Auth;

public class VerifyOtpRequestDto
{
    [Required]
    [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Mobile number must be 10 digits")]
    public string MobileNumber { get; set; } = null!;

    [Required]
    [RegularExpression(@"^[0-9]{6}$", ErrorMessage = "OTP must be 6 digits")]
    public string Otp { get; set; } = null!;
} 