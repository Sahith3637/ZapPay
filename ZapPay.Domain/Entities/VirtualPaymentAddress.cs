using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ZapPay.Domain.Entities;

[Index("IsDeleted", Name = "IX_VPA_IsDeleted")]
[Index("Vpa", Name = "UQ_VPA_Address", IsUnique = true)]
public partial class VirtualPaymentAddress
{
    [Key]
    [Column("VPAId")]
    public Guid Vpaid { get; set; }

    public Guid UserId { get; set; }

    [Column("VPA")]
    [StringLength(100)]
    public string Vpa { get; set; } = null!;

    [StringLength(20)]
    public string Status { get; set; } = null!;

    public bool? IsDefault { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid UpdatedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("VirtualPaymentAddressCreatedByNavigations")]
    public virtual SystemUser CreatedByNavigation { get; set; } = null!;

    [ForeignKey("UpdatedBy")]
    [InverseProperty("VirtualPaymentAddressUpdatedByNavigations")]
    public virtual SystemUser UpdatedByNavigation { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("VirtualPaymentAddresses")]
    public virtual User User { get; set; } = null!;
}
