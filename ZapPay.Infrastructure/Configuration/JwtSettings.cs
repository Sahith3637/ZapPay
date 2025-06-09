using System;

namespace ZapPay.Infrastructure.Configuration;

public class JwtSettings
{
    public string Secret { get; set; } = null!;
    public int ExpiryInHours { get; set; } = 24;
} 