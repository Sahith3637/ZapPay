using System;

namespace ZapPay.Application.DTOs;

public class CreateBankAccountDto
{
    public string AccountNumber { get; set; }
    public string IFSC { get; set; }
    public string BankName { get; set; }
    public string AccountType { get; set; } // Savings, Current
    public decimal InitialBalance { get; set; }
}

public class BankAccountResponseDto
{
    public Guid AccountId { get; set; }
    public Guid UserId { get; set; }
    public string AccountNumber { get; set; }
    public string IFSC { get; set; }
    public string BankName { get; set; }
    public string AccountType { get; set; }
    public string Status { get; set; }
    public bool IsDefault { get; set; }
    public decimal Balance { get; set; }
    public DateTime CreatedAt { get; set; }
} 