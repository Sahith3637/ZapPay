using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ZapPay.Domain.Entities;

[Index("IsDeleted", Name = "IX_BankAccounts_IsDeleted")]
public partial class BankAccount
{
    [Key]
    public Guid AccountId { get; set; }

    public Guid UserId { get; set; }

    [StringLength(50)]
    public string AccountNumber { get; set; } = null!;

    [Column("IFSC")]
    [StringLength(11)]
    public string Ifsc { get; set; } = null!;

    [StringLength(100)]
    public string BankName { get; set; } = null!;

    [StringLength(20)]
    public string AccountType { get; set; } = null!;

    [StringLength(20)]
    public string Status { get; set; } = null!;

    public bool? IsDefault { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid UpdatedBy { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Balance { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("BankAccountCreatedByNavigations")]
    public virtual SystemUser CreatedByNavigation { get; set; } = null!;

    [ForeignKey("UpdatedBy")]
    [InverseProperty("BankAccountUpdatedByNavigations")]
    public virtual SystemUser UpdatedByNavigation { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("BankAccounts")]
    public virtual User User { get; set; } = null!;
}
