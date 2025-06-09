namespace ZapPay.Application.DTOs;

public class SendVpaRequestDto
{
    public string FromVpa { get; set; } = null!;
    public string ToVpa { get; set; } = null!;
    public decimal Amount { get; set; }
    public string? Remarks { get; set; }
} 