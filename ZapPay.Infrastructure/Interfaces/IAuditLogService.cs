namespace ZapPay.Infrastructure.Interfaces;

public interface IAuditLogService
{
    Task LogAsync(
        Guid userId,
        string action,
        string entityType,
        Guid entityId,
        string? oldValue = null,
        string? newValue = null);
} 