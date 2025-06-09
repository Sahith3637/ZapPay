using System;
using System.Threading.Tasks;
using AutoMapper;
using ZapPay.Application.DTOs;
using ZapPay.Application.Interfaces;
using ZapPay.Domain.Entities;
using ZapPay.Persistence.Interfaces;

namespace ZapPay.Application.Services;

public class SendMoneyService : ISendMoneyService
{
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMapper _mapper;
    private static readonly Guid SYSTEM_USER_ID = new("00000000-0000-0000-0000-000000000001");

    public SendMoneyService(
        IBankAccountRepository bankAccountRepository,
        ITransactionRepository transactionRepository,
        IMapper mapper)
    {
        _bankAccountRepository = bankAccountRepository;
        _transactionRepository = transactionRepository;
        _mapper = mapper;
    }

    public async Task<SendVpaResponseDto> SendByVpaAsync(SendVpaRequestDto dto, Guid systemUserId)
    {
        // Validate input
        if (dto.Amount <= 0)
            throw new ArgumentException("Amount must be greater than zero");

        // Lookup sender and receiver accounts
        var senderAccount = await _bankAccountRepository.GetByVpaAsync(dto.FromVpa);
        var receiverAccount = await _bankAccountRepository.GetByVpaAsync(dto.ToVpa);
        if (senderAccount == null) throw new ArgumentException("Sender VPA not found");
        if (receiverAccount == null) throw new ArgumentException("Receiver VPA not found");
        if (senderAccount.Balance < dto.Amount) throw new InvalidOperationException("Insufficient balance");

        // Update balances
        senderAccount.Balance -= dto.Amount;
        receiverAccount.Balance += dto.Amount;
        await _bankAccountRepository.UpdateBalanceAsync(senderAccount.AccountId, senderAccount.Balance);
        await _bankAccountRepository.UpdateBalanceAsync(receiverAccount.AccountId, receiverAccount.Balance);

        // Create transaction
        var transaction = new Transaction
        {
            TransactionId = Guid.NewGuid(),
            UserId = senderAccount.UserId,
            TransactionType = "Send",
            Amount = dto.Amount,
            Currency = "INR",
            Status = "Success",
            SourceType = "VPA",
            SourceIdentifier = dto.FromVpa,
            DestinationType = "VPA",
            DestinationIdentifier = dto.ToVpa,
            Remarks = dto.Remarks,
            IsRefund = false,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = SYSTEM_USER_ID,
            UpdatedAt = DateTime.UtcNow,
            UpdatedBy = SYSTEM_USER_ID
        };
        await _transactionRepository.AddTransactionAsync(transaction);

        // Map to response
        return _mapper.Map<SendVpaResponseDto>(transaction);
    }
} 