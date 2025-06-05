using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ZapPay.Domain.Entities;

public partial class TransactionLog
{
    [Key]
    public Guid LogId { get; set; }

    public Guid TransactionId { get; set; }

    [StringLength(20)]
    public string Status { get; set; } = null!;

    [StringLength(255)]
    public string? Remarks { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid CreatedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("TransactionLogs")]
    public virtual SystemUser CreatedByNavigation { get; set; } = null!;

    [ForeignKey("TransactionId")]
    [InverseProperty("TransactionLogs")]
    public virtual Transaction Transaction { get; set; } = null!;
}
