using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ZapPay.Application.DTOs;

public class AddKycWithFileRequestDto
{
    [Required]
    public string DocumentType { get; set; } = null!;

    [Required]
    public string DocumentNumber { get; set; } = null!;

    [Required]
    public IFormFile DocumentFile { get; set; } = null!;
} 