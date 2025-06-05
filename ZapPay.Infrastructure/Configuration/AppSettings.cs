using System.Collections.Generic;

namespace ZapPay.Infrastructure.Configuration;

public class AppSettings
{
    public ConnectionStrings ConnectionStrings { get; set; } = new();
    public EncryptionSettings Encryption { get; set; } = new();
    public JwtSettings Jwt { get; set; } = new();
    public KycSettings KYC { get; set; } = new();
    public UserSettings User { get; set; } = new();
    public SystemUserSettings SystemUser { get; set; } = new();
}

public class ConnectionStrings
{
    public string DefaultConnection { get; set; } = string.Empty;
}

public class EncryptionSettings
{
    public string Key { get; set; } = string.Empty;
    public string IV { get; set; } = string.Empty;
}

public class KycSettings
{
    public int MaxRetryAttempts { get; set; }
    public List<string> DocumentTypes { get; set; } = new();
    public List<string> VerificationStatuses { get; set; } = new();
}

public class UserSettings
{
    public List<string> Statuses { get; set; } = new();
    public List<string> KYCStatuses { get; set; } = new();
}

public class SystemUserSettings
{
    public string DefaultSystemUserId { get; set; } = string.Empty;
} 