using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ZapPay.Domain.Entities;

public partial class Notification
{
    [Key]
    public Guid NotificationId { get; set; }

    public Guid UserId { get; set; }

    [StringLength(50)]
    public string Type { get; set; } = null!;

    [StringLength(20)]
    public string Channel { get; set; } = null!;

    [StringLength(20)]
    public string Status { get; set; } = null!;

    public string Content { get; set; } = null!;

    [StringLength(100)]
    public string? TemplateId { get; set; }

    public string? TemplateParameters { get; set; }

    public int? RetryCount { get; set; }

    public DateTime? LastRetryAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? SentAt { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("Notifications")]
    public virtual SystemUser CreatedByNavigation { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Notifications")]
    public virtual User User { get; set; } = null!;
}
