using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ZapPay.Domain.Entities;

[Index("CreatedAt", Name = "IX_AuditLogs_CreatedAt")]
[Index("UserId", Name = "IX_AuditLogs_UserId")]
public partial class AuditLog
{
    [Key]
    public Guid LogId { get; set; }

    public Guid UserId { get; set; }

    [StringLength(100)]
    public string Action { get; set; } = null!;

    [StringLength(50)]
    public string EntityType { get; set; } = null!;

    public Guid EntityId { get; set; }

    public string? OldValue { get; set; }

    public string? NewValue { get; set; }

    [Column("IPAddress")]
    [StringLength(45)]
    public string? Ipaddress { get; set; }

    public string? UserAgent { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid CreatedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("AuditLogs")]
    public virtual SystemUser CreatedByNavigation { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("AuditLogs")]
    public virtual User User { get; set; } = null!;
}
