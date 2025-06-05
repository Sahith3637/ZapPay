using System.ComponentModel.DataAnnotations;

namespace ZapPay.Application.DTOs;

public class RegisterUserRequestDto
{
    [Required]
    [StringLength(10)]
    public string MobileNumber { get; set; } = null!;

    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = null!;

    [Required]
    public DateOnly DateOfBirth { get; set; }
} 