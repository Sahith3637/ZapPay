using ZapPay.Domain.Entities;

namespace ZapPay.Persistence.Interfaces;
 
public interface IUserRepository : IGenericRepository<User>
{
    Task<bool> ExistsByMobileNumberAsync(string mobileNumber);
    Task<bool> ExistsByEmailAsync(string email);
    Task<User?> GetByMobileNumberAsync(string mobileNumber);
    Task UpdateProfileAsync(Guid userId, string firstName, string lastName, string email);
    Task<User?> GetByIdAsync(Guid userId, bool includeVpa);
} 