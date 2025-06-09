using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ZapPay.Domain.Entities;

[Table("QRCodeDetails")]
[Index("Status", Name = "IX_QRCodeDetails_Status")]
[Index("UserId", Name = "IX_QRCodeDetails_UserId")]
public partial class QrcodeDetail
{
    [Key]
    [Column("QRCodeId")]
    public Guid QrcodeId { get; set; }

    public Guid UserId { get; set; }

    [Column("QRCodeType")]
    [StringLength(20)]
    public string QrcodeType { get; set; } = null!;

    [Column(TypeName = "decimal(15, 2)")]
    public decimal? Amount { get; set; }

    [StringLength(100)]
    public string? Purpose { get; set; }

    [Column("QRCodeData")]
    public string QrcodeData { get; set; } = null!;

    [StringLength(20)]
    public string Status { get; set; } = null!;

    public DateTime? ExpiresAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid UpdatedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("QrcodeDetailCreatedByNavigations")]
    public virtual SystemUser CreatedByNavigation { get; set; } = null!;

    [InverseProperty("Qrcode")]
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    [ForeignKey("UpdatedBy")]
    [InverseProperty("QrcodeDetailUpdatedByNavigations")]
    public virtual SystemUser UpdatedByNavigation { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("QrcodeDetails")]
    public virtual User User { get; set; } = null!;
}
