using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ZapPay.Domain.Entities;

[Index("Email", Name = "IX_Users_Email")]
[Index("IsDeleted", Name = "IX_Users_IsDeleted")]
[Index("MobileNumber", Name = "IX_Users_MobileNumber")]
[Index("Status", Name = "IX_Users_Status")]
[Index("Email", Name = "UQ_Users_Email", IsUnique = true)]
[Index("MobileNumber", Name = "UQ_Users_MobileNumber", IsUnique = true)]
public partial class User
{
    [Key]
    public Guid UserId { get; set; }

    [StringLength(15)]
    public string MobileNumber { get; set; } = null!;

    [StringLength(255)]
    public string? Email { get; set; }

    [StringLength(100)]
    public string? FirstName { get; set; }

    [StringLength(100)]
    public string? LastName { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    [StringLength(20)]
    public string Status { get; set; } = null!;

    [Column("KYCStatus")]
    [StringLength(20)]
    public string Kycstatus { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid UpdatedBy { get; set; }

    public DateTime? LastLoginAt { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    [InverseProperty("User")]
    public virtual ICollection<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();

    [ForeignKey("CreatedBy")]
    [InverseProperty("UserCreatedByNavigations")]
    public virtual SystemUser CreatedByNavigation { get; set; } = null!;

    [InverseProperty("User")]
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    [InverseProperty("User")]
    public virtual ICollection<Otpverification> Otpverifications { get; set; } = new List<Otpverification>();

    [InverseProperty("User")]
    public virtual ICollection<QrcodeDetail> QrcodeDetails { get; set; } = new List<QrcodeDetail>();

    [InverseProperty("User")]
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    [ForeignKey("UpdatedBy")]
    [InverseProperty("UserUpdatedByNavigations")]
    public virtual SystemUser UpdatedByNavigation { get; set; } = null!;

    [InverseProperty("User")]
    public virtual ICollection<Upimandate> Upimandates { get; set; } = new List<Upimandate>();

    [InverseProperty("User")]
    public virtual ICollection<UserKyc> UserKycs { get; set; } = new List<UserKyc>();

    [InverseProperty("User")]
    public virtual ICollection<UserSession> UserSessions { get; set; } = new List<UserSession>();

    [InverseProperty("User")]
    public virtual ICollection<VirtualPaymentAddress> VirtualPaymentAddresses { get; set; } = new List<VirtualPaymentAddress>();
}
