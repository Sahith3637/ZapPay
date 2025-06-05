using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ZapPay.Domain.Entities;

[Index("CreatedAt", Name = "IX_Transactions_CreatedAt")]
[Index("IsDeleted", Name = "IX_Transactions_IsDeleted")]
[Index("OriginalTransactionId", Name = "IX_Transactions_OriginalTransactionId")]
[Index("QrcodeId", Name = "IX_Transactions_QRCodeId")]
[Index("ReferenceId", Name = "IX_Transactions_ReferenceId")]
[Index("Status", Name = "IX_Transactions_Status")]
[Index("UserId", Name = "IX_Transactions_UserId")]
[Index("ReferenceId", Name = "UQ_Transactions_ReferenceId", IsUnique = true)]
public partial class Transaction
{
    [Key]
    public Guid TransactionId { get; set; }

    public Guid UserId { get; set; }

    [StringLength(20)]
    public string TransactionType { get; set; } = null!;

    [Column(TypeName = "decimal(15, 2)")]
    public decimal Amount { get; set; }

    [StringLength(3)]
    public string? Currency { get; set; }

    [StringLength(20)]
    public string Status { get; set; } = null!;

    [StringLength(20)]
    public string SourceType { get; set; } = null!;

    [StringLength(100)]
    public string SourceIdentifier { get; set; } = null!;

    [StringLength(20)]
    public string DestinationType { get; set; } = null!;

    [StringLength(100)]
    public string DestinationIdentifier { get; set; } = null!;

    [StringLength(100)]
    public string? ReferenceId { get; set; }

    [Column("NPCIReferenceId")]
    [StringLength(100)]
    public string? NpcireferenceId { get; set; }

    [StringLength(255)]
    public string? Remarks { get; set; }

    public bool? IsRefund { get; set; }

    public Guid? OriginalTransactionId { get; set; }

    [Column("QRCodeId")]
    public Guid? QrcodeId { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid UpdatedBy { get; set; }

    public DateTime? CompletedAt { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("TransactionCreatedByNavigations")]
    public virtual SystemUser CreatedByNavigation { get; set; } = null!;

    [InverseProperty("OriginalTransaction")]
    public virtual ICollection<Transaction> InverseOriginalTransaction { get; set; } = new List<Transaction>();

    [ForeignKey("OriginalTransactionId")]
    [InverseProperty("InverseOriginalTransaction")]
    public virtual Transaction? OriginalTransaction { get; set; }

    [ForeignKey("QrcodeId")]
    [InverseProperty("Transactions")]
    public virtual QrcodeDetail? Qrcode { get; set; }

    [InverseProperty("Transaction")]
    public virtual ICollection<TransactionLog> TransactionLogs { get; set; } = new List<TransactionLog>();

    [ForeignKey("UpdatedBy")]
    [InverseProperty("TransactionUpdatedByNavigations")]
    public virtual SystemUser UpdatedByNavigation { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Transactions")]
    public virtual User User { get; set; } = null!;
}
