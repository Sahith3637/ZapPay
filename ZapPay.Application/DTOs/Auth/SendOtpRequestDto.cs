using System.ComponentModel.DataAnnotations;

namespace ZapPay.Application.DTOs.Auth;

public class SendOtpRequestDto
{
    [Required]
    [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Mobile number must be 10 digits")]
    public string MobileNumber { get; set; } = null!;
} 