using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ZapPay.Domain.Entities;

[Index("Status", Name = "IX_UserSessions_Status")]
[Index("UserId", Name = "IX_UserSessions_UserId")]
public partial class UserSession
{
    [Key]
    public Guid SessionId { get; set; }

    public Guid UserId { get; set; }

    public string Token { get; set; } = null!;

    public string? RefreshToken { get; set; }

    [StringLength(100)]
    public string? DeviceId { get; set; }

    [Column("IPAddress")]
    [StringLength(45)]
    public string? Ipaddress { get; set; }

    public string? UserAgent { get; set; }

    [StringLength(20)]
    public string LoginType { get; set; } = null!;

    [StringLength(20)]
    public string Status { get; set; } = null!;

    public DateTime ExpiresAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid UpdatedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("UserSessionCreatedByNavigations")]
    public virtual SystemUser CreatedByNavigation { get; set; } = null!;

    [ForeignKey("UpdatedBy")]
    [InverseProperty("UserSessionUpdatedByNavigations")]
    public virtual SystemUser UpdatedByNavigation { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("UserSessions")]
    public virtual User User { get; set; } = null!;
}
