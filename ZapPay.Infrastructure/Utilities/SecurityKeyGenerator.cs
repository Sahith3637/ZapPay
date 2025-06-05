using System.Security.Cryptography;

namespace ZapPay.Infrastructure.Utilities;

public static class SecurityKeyGenerator
{
    public static string GenerateAesKey()
    {
        using var aes = Aes.Create();
        aes.GenerateKey();
        return Convert.ToBase64String(aes.Key);
    }

    public static string GenerateAesIV()
    {
        using var aes = Aes.Create();
        aes.GenerateIV();
        return Convert.ToBase64String(aes.IV);
    }

    public static string GenerateJwtSecret()
    {
        var randomBytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
} 