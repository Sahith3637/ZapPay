using Microsoft.EntityFrameworkCore;
using ZapPay.Domain.Entities;
using ZapPay.Persistence.Context;
using ZapPay.Persistence.Interfaces;

namespace ZapPay.Persistence.Repositories;

public class VpaRepository : IVpaRepository
{
    private readonly AppDbContext _context;
    public VpaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> VpaExistsAsync(string vpa)
    {
        return await _context.VirtualPaymentAddresses.AnyAsync(x => x.Vpa == vpa);
    }

    public async Task AddVpaAsync(VirtualPaymentAddress vpa)
    {
        await _context.VirtualPaymentAddresses.AddAsync(vpa);
        await _context.SaveChangesAsync();
    }

    public async Task<VirtualPaymentAddress?> GetByVpaAsync(string vpa)
    {
        return await _context.VirtualPaymentAddresses.FirstOrDefaultAsync(x => x.Vpa == vpa);
    }
} 