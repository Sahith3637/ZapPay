using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZapPay.Domain.Entities;
using ZapPay.Persistence.Context;
using ZapPay.Persistence.Interfaces;

namespace ZapPay.Persistence.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _context;
    public TransactionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddTransactionAsync(Transaction transaction)
    {
        await _context.Transactions.AddAsync(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task<Transaction?> GetByIdAsync(Guid transactionId)
    {
        return await _context.Transactions.FirstOrDefaultAsync(t => t.TransactionId == transactionId);
    }
} 