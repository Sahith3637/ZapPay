using System.Threading.Tasks;
using ZapPay.Domain.Entities;

namespace ZapPay.Persistence.Interfaces;

public interface IVpaRepository
{
    Task<bool> VpaExistsAsync(string vpa);
    Task AddVpaAsync(VirtualPaymentAddress vpa);
    Task<VirtualPaymentAddress?> GetByVpaAsync(string vpa);
} 