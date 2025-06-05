using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ZapPay.Domain.Entities;

public partial class SystemUser
{
    [Key]
    public Guid SystemUserId { get; set; }

    [StringLength(20)]
    public string UserType { get; set; } = null!;

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [StringLength(255)]
    public string? Description { get; set; }

    [StringLength(20)]
    public string Status { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid UpdatedBy { get; set; }

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<BankAccount> BankAccountCreatedByNavigations { get; set; } = new List<BankAccount>();

    [InverseProperty("UpdatedByNavigation")]
    public virtual ICollection<BankAccount> BankAccountUpdatedByNavigations { get; set; } = new List<BankAccount>();

    [ForeignKey("CreatedBy")]
    [InverseProperty("InverseCreatedByNavigation")]
    public virtual SystemUser CreatedByNavigation { get; set; } = null!;

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<SystemUser> InverseCreatedByNavigation { get; set; } = new List<SystemUser>();

    [InverseProperty("UpdatedByNavigation")]
    public virtual ICollection<SystemUser> InverseUpdatedByNavigation { get; set; } = new List<SystemUser>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Otpverification> OtpverificationCreatedByNavigations { get; set; } = new List<Otpverification>();

    [InverseProperty("UpdatedByNavigation")]
    public virtual ICollection<Otpverification> OtpverificationUpdatedByNavigations { get; set; } = new List<Otpverification>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<QrcodeDetail> QrcodeDetailCreatedByNavigations { get; set; } = new List<QrcodeDetail>();

    [InverseProperty("UpdatedByNavigation")]
    public virtual ICollection<QrcodeDetail> QrcodeDetailUpdatedByNavigations { get; set; } = new List<QrcodeDetail>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Transaction> TransactionCreatedByNavigations { get; set; } = new List<Transaction>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<TransactionLog> TransactionLogs { get; set; } = new List<TransactionLog>();

    [InverseProperty("UpdatedByNavigation")]
    public virtual ICollection<Transaction> TransactionUpdatedByNavigations { get; set; } = new List<Transaction>();

    [ForeignKey("UpdatedBy")]
    [InverseProperty("InverseUpdatedByNavigation")]
    public virtual SystemUser UpdatedByNavigation { get; set; } = null!;

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<Upimandate> UpimandateCreatedByNavigations { get; set; } = new List<Upimandate>();

    [InverseProperty("UpdatedByNavigation")]
    public virtual ICollection<Upimandate> UpimandateUpdatedByNavigations { get; set; } = new List<Upimandate>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<User> UserCreatedByNavigations { get; set; } = new List<User>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<UserKyc> UserKycCreatedByNavigations { get; set; } = new List<UserKyc>();

    [InverseProperty("UpdatedByNavigation")]
    public virtual ICollection<UserKyc> UserKycUpdatedByNavigations { get; set; } = new List<UserKyc>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<UserSession> UserSessionCreatedByNavigations { get; set; } = new List<UserSession>();

    [InverseProperty("UpdatedByNavigation")]
    public virtual ICollection<UserSession> UserSessionUpdatedByNavigations { get; set; } = new List<UserSession>();

    [InverseProperty("UpdatedByNavigation")]
    public virtual ICollection<User> UserUpdatedByNavigations { get; set; } = new List<User>();

    [InverseProperty("CreatedByNavigation")]
    public virtual ICollection<VirtualPaymentAddress> VirtualPaymentAddressCreatedByNavigations { get; set; } = new List<VirtualPaymentAddress>();

    [InverseProperty("UpdatedByNavigation")]
    public virtual ICollection<VirtualPaymentAddress> VirtualPaymentAddressUpdatedByNavigations { get; set; } = new List<VirtualPaymentAddress>();
}
