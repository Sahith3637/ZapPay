using ZapPay.Domain.Entities;
using ZapPay.Persistence.Context;
using ZapPay.Persistence.Interfaces;

namespace ZapPay.Persistence.Repositories;

public class KycRepository : GenericRepository<UserKyc>, IKycRepository
{
    public KycRepository(AppDbContext context) : base(context)
    {
    }
} 