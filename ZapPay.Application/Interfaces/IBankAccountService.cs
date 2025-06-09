using System;
using System.Threading.Tasks;
using ZapPay.Application.DTOs;

namespace ZapPay.Application.Interfaces;

public interface IBankAccountService
{
    Task<BankAccountResponseDto> CreateBankAccountAsync(Guid userId, CreateBankAccountDto dto);
    Task<BankAccountResponseDto> GetBankAccountAsync(Guid accountId);
    Task<BankAccountResponseDto> GetDefaultBankAccountAsync(Guid userId);
    Task SetDefaultBankAccountAsync(Guid userId, Guid accountId);
} 