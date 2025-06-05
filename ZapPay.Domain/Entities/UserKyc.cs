using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ZapPay.Domain.Entities;

[Table("UserKYC")]
[Index("IsDeleted", Name = "IX_UserKYC_IsDeleted")]
[Index("RetryCount", "LastAttemptAt", Name = "IX_UserKYC_RetryCount")]
public partial class UserKyc
{
    [Key]
    [Column("KYCId")]
    public Guid Kycid { get; set; }

    public Guid UserId { get; set; }

    [StringLength(20)]
    public string DocumentType { get; set; } = null!;

    [StringLength(60)]
    public string DocumentNumber { get; set; } = null!;

    [StringLength(255)]
    public string DocumentHash { get; set; } = null!;

    [StringLength(20)]
    public string VerificationStatus { get; set; } = null!;

    [StringLength(255)]
    public string? VerificationRemarks { get; set; }

    public int RetryCount { get; set; }

    public DateTime? LastAttemptAt { get; set; }

    public int MaxRetries { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid UpdatedBy { get; set; }

    [StringLength(500)]
    public string? DocumentFilePath { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("UserKycCreatedByNavigations")]
    public virtual SystemUser CreatedByNavigation { get; set; } = null!;

    [ForeignKey("UpdatedBy")]
    [InverseProperty("UserKycUpdatedByNavigations")]
    public virtual SystemUser UpdatedByNavigation { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("UserKycs")]
    public virtual User User { get; set; } = null!;
}
