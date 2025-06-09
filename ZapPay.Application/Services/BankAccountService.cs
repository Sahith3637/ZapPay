using System;
using System.Threading.Tasks;
using AutoMapper;
using ZapPay.Application.DTOs;
using ZapPay.Application.Interfaces;
using ZapPay.Domain.Entities;
using ZapPay.Persistence.Interfaces;

namespace ZapPay.Application.Services;

public class BankAccountService : IBankAccountService
{
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly IMapper _mapper;
    private static readonly Guid SYSTEM_USER_ID = new("00000000-0000-0000-0000-000000000001");

    public BankAccountService(IBankAccountRepository bankAccountRepository, IMapper mapper)
    {
        _bankAccountRepository = bankAccountRepository;
        _mapper = mapper;
    }

    public async Task<BankAccountResponseDto> CreateBankAccountAsync(Guid userId, CreateBankAccountDto dto)
    {
        // Check if user already has a default account
        var existingDefault = await _bankAccountRepository.GetDefaultAccountAsync(userId);
        var isDefault = existingDefault == null;

        // Map DTO to entity
        var bankAccount = _mapper.Map<BankAccount>(dto);
        
        // Set additional properties
        bankAccount.UserId = userId;
        bankAccount.IsDefault = isDefault;
        bankAccount.CreatedBy = SYSTEM_USER_ID;
        bankAccount.UpdatedBy = SYSTEM_USER_ID;

        await _bankAccountRepository.AddBankAccountAsync(bankAccount);
        return _mapper.Map<BankAccountResponseDto>(bankAccount);
    }

    public async Task<BankAccountResponseDto> GetBankAccountAsync(Guid accountId)
    {
        var account = await _bankAccountRepository.GetByIdAsync(accountId);
        return _mapper.Map<BankAccountResponseDto>(account);
    }

    public async Task<BankAccountResponseDto> GetDefaultBankAccountAsync(Guid userId)
    {
        var account = await _bankAccountRepository.GetDefaultAccountAsync(userId);
        return _mapper.Map<BankAccountResponseDto>(account);
    }

    public async Task SetDefaultBankAccountAsync(Guid userId, Guid accountId)
    {
        // Get current default account
        var currentDefault = await _bankAccountRepository.GetDefaultAccountAsync(userId);
        if (currentDefault != null)
        {
            currentDefault.IsDefault = false;
            await _bankAccountRepository.UpdateBankAccountAsync(currentDefault);
        }

        // Set new default account
        var newDefault = await _bankAccountRepository.GetByIdAsync(accountId);
        if (newDefault != null)
        {
            newDefault.IsDefault = true;
            await _bankAccountRepository.UpdateBankAccountAsync(newDefault);
        }
    }
} 