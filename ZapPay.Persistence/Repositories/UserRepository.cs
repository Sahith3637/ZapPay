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

    public async Task<User?> GetByMobileNumberAsync(string mobileNumber)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.MobileNumber == mobileNumber && !u.IsDeleted);
    }

    public async Task UpdateProfileAsync(Guid userId, string firstName, string lastName, string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId && !u.IsDeleted);
        if (user == null) throw new KeyNotFoundException("User not found");
        user.FirstName = firstName;
        user.LastName = lastName;
        user.Email = email;
        user.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }

    public async Task<User?> GetByIdAsync(Guid userId, bool includeVpa)
    {
        if (includeVpa)
        {
            return await _context.Users
                .Include(u => u.VirtualPaymentAddresses)
                .FirstOrDefaultAsync(u => u.UserId == userId && !u.IsDeleted);
        }
        else
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId && !u.IsDeleted);
        }
    }
} 