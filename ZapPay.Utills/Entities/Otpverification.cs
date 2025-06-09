using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ZapPay.Domain.Entities;

[Table("OTPVerifications")]
[Index("ContactValue", Name = "IX_OTPVerifications_ContactValue")]
[Index("Status", Name = "IX_OTPVerifications_Status")]
[Index("UserId", Name = "IX_OTPVerifications_UserId")]
public partial class Otpverification
{
    [Key]
    [Column("OTPId")]
    public Guid Otpid { get; set; }

    public Guid UserId { get; set; }

    [Column("OTPType")]
    [StringLength(20)]
    public string Otptype { get; set; } = null!;

    [Column("OTPValue")]
    [StringLength(10)]
    public string Otpvalue { get; set; } = null!;

    [StringLength(20)]
    public string ContactType { get; set; } = null!;

    [StringLength(100)]
    public string ContactValue { get; set; } = null!;

    [StringLength(20)]
    public string Status { get; set; } = null!;

    public DateTime ExpiresAt { get; set; }

    public DateTime? VerifiedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid UpdatedBy { get; set; }

    [ForeignKey("CreatedBy")]
    [InverseProperty("OtpverificationCreatedByNavigations")]
    public virtual SystemUser CreatedByNavigation { get; set; } = null!;

    [ForeignKey("UpdatedBy")]
    [InverseProperty("OtpverificationUpdatedByNavigations")]
    public virtual SystemUser UpdatedByNavigation { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("Otpverifications")]
    public virtual User User { get; set; } = null!;
}
