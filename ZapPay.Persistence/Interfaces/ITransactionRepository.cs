using System;
using System.Threading.Tasks;
using ZapPay.Domain.Entities;

namespace ZapPay.Persistence.Interfaces;

public interface ITransactionRepository
{
    Task AddTransactionAsync(Transaction transaction);
    Task<Transaction?> GetByIdAsync(Guid transactionId);
    // Add more methods as needed
} 