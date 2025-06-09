using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ZapPay.Domain.Entities;
using ZapPay.Persistence.Context;
using ZapPay.Persistence.Interfaces;

namespace ZapPay.Persistence.Repositories;

public class BankAccountRepository : IBankAccountRepository
{
    private readonly AppDbContext _context;
    public BankAccountRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<BankAccount?> GetByIdAsync(Guid accountId)
    {
        return await _context.BankAccounts
            .FirstOrDefaultAsync(b => b.AccountId == accountId && !b.IsDeleted);
    }

    public async Task<BankAccount?> GetDefaultAccountAsync(Guid userId)
    {
        return await _context.BankAccounts
            .FirstOrDefaultAsync(b => b.UserId == userId && b.IsDefault == true && b.IsDeleted == false);
    }

    public async Task AddBankAccountAsync(BankAccount account)
    {
        await _context.BankAccounts.AddAsync(account);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateBankAccountAsync(BankAccount account)
    {
        _context.BankAccounts.Update(account);
        await _context.SaveChangesAsync();
    }

    public async Task<BankAccount?> GetByVpaAsync(string vpa)
    {
        return await _context.BankAccounts
            .Include(b => b.User)
            .FirstOrDefaultAsync(b => b.User.VirtualPaymentAddresses.Any(v => v.Vpa == vpa));
    }

    public async Task UpdateBalanceAsync(Guid accountId, decimal newBalance)
    {
        var account = await _context.BankAccounts.FirstOrDefaultAsync(b => b.AccountId == accountId);
        if (account != null)
        {
            account.Balance = newBalance;
            await _context.SaveChangesAsync();
        }
    }
} 