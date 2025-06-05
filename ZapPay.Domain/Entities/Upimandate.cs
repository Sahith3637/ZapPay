using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ZapPay.Domain.Entities;

[Table("UPIMandates")]
[Index("Status", Name = "IX_UPIMandates_Status")]
[Index("UserId", Name = "IX_UPIMandates_UserId")]
[Index("ReferenceId", Name = "UQ_UPIMandates_ReferenceId", IsUnique = true)]
public partial class Upimandate
{
    [Key]
    public Guid MandateId { get; set; }

    public Guid UserId { get; set; }

    [Column(TypeName = "decimal(15, 2)")]
    public decimal Amount { get; set; }

    [StringLength(20)]
    public string Frequency { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    [StringLength(20)]
    public string Status { get; set; } = null!;

    [StringLength(100)]
    public string? ReferenceId { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid UpdatedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("UpimandateCreatedByNavigations")]
    public virtual SystemUser CreatedByNavigation { get; set; } = null!;

    [ForeignKey("UpdatedBy")]
    [InverseProperty("UpimandateUpdatedByNavigations")]
    public virtual SystemUser UpdatedByNavigation { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Upimandates")]
    public virtual User User { get; set; } = null!;
}
