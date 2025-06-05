using System.ComponentModel.DataAnnotations;

namespace ZapPay.Application.DTOs;

public class ResubmitKycRequestDto
{
    [Required]
    [StringLength(50)]
    public string DocumentNumber { get; set; } = null!;

    [StringLength(255)]
    public string? DocumentHash { get; set; }
} 