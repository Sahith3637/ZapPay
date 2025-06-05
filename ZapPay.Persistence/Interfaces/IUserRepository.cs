using ZapPay.Domain.Entities;

namespace ZapPay.Persistence.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    Task<bool> ExistsByMobileNumberAsync(string mobileNumber);
    Task<bool> ExistsByEmailAsync(string email);
} 