namespace ZapPay.Application.DTOs;

public class RegisterUserResponseDto
{
    public Guid UserId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string MobileNumber { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateOnly DateOfBirth { get; set; }
    public string Status { get; set; } = null!;
    public string KYCStatus { get; set; } = null!;
    public string? KYCVerificationRemarks { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Vpa { get; set; }
} 