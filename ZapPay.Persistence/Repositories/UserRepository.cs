using Microsoft.EntityFrameworkCore;
using ZapPay.Domain.Entities;
using ZapPay.Persistence.Context;
using ZapPay.Persistence.Interfaces;

namespace ZapPay.Persistence.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<bool> ExistsByMobileNumberAsync(string mobileNumber)
    {
        return await _context.Users
            .AnyAsync(u => u.MobileNumber == mobileNumber && !u.IsDeleted);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _context.Users
            .AnyAsync(u => u.Email == email && !u.IsDeleted);
    }
} 