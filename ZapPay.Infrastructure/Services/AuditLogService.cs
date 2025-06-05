using Microsoft.AspNetCore.Http;
using ZapPay.Domain.Entities;
using ZapPay.Infrastructure.Interfaces;
using ZapPay.Persistence.Interfaces;

namespace ZapPay.Infrastructure.Services;

public class AuditLogService : IAuditLogService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IGenericRepository<AuditLog> _auditLogRepository;
    private readonly IGenericRepository<SystemUser> _systemUserRepository;

    public AuditLogService(
        IHttpContextAccessor httpContextAccessor,
        IGenericRepository<AuditLog> auditLogRepository,
        IGenericRepository<SystemUser> systemUserRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _auditLogRepository = auditLogRepository;
        _systemUserRepository = systemUserRepository;
    }

    public async Task LogAsync(
        Guid userId,
        string action,
        string entityType,
        Guid entityId,
        string? oldValue = null,
        string? newValue = null)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var systemUserId = await GetSystemUserIdAsync();

        var auditLog = new AuditLog
        {
            UserId = userId,
            Action = action,
            EntityType = entityType,
            EntityId = entityId,
            OldValue = oldValue,
            NewValue = newValue,
            Ipaddress = httpContext?.Connection?.RemoteIpAddress?.ToString(),
            UserAgent = httpContext?.Request.Headers["User-Agent"].ToString(),
            CreatedAt = DateTime.UtcNow,
            CreatedBy = systemUserId
        };

        await _auditLogRepository.AddAsync(auditLog);
        await _auditLogRepository.SaveChangesAsync();
    }

    private async Task<Guid> GetSystemUserIdAsync()
    {
        // In a real application, you would get this from the current user's claims
        // For now, we'll use a default system user ID
        return Guid.Parse("00000000-0000-0000-0000-000000000001");
    }
} 