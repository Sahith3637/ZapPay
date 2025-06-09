namespace ZapPay.Application.DTOs;

public class SendVpaResponseDto
{
    public Guid TransactionId { get; set; }
    public string Status { get; set; } = null!;
    public decimal Amount { get; set; }
    public string FromVpa { get; set; } = null!;
    public string ToVpa { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public string? Remarks { get; set; }
} 