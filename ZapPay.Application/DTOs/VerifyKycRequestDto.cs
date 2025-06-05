using System.ComponentModel.DataAnnotations;

namespace ZapPay.Application.DTOs;

public class VerifyKycRequestDto
{
    [Required]
    [StringLength(20)]
    public string VerificationStatus { get; set; } = null!;

    [StringLength(255)]
    public string? VerificationRemarks { get; set; }
} 