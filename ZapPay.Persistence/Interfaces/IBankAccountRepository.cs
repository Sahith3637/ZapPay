using System;
using System.Threading.Tasks;
using ZapPay.Domain.Entities;

namespace ZapPay.Persistence.Interfaces;

public interface IBankAccountRepository
{
    Task<BankAccount?> GetByIdAsync(Guid accountId);
    Task<BankAccount?> GetDefaultAccountAsync(Guid userId);
    Task AddBankAccountAsync(BankAccount account);
    Task UpdateBankAccountAsync(BankAccount account);
    Task<BankAccount?> GetByVpaAsync(string vpa);
    Task UpdateBalanceAsync(Guid accountId, decimal newBalance);
    // Add more methods as needed
} 