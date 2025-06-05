create database ZapPay;

CREATE TABLE SystemUsers (
    SystemUserId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserType NVARCHAR(20) NOT NULL, -- SYSTEM, ADMIN, USER, API, BATCH
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    Status NVARCHAR(20) NOT NULL, -- Active, Inactive
    IsDeleted BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy UNIQUEIDENTIFIER NOT NULL,
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedBy UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT FK_SystemUsers_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES SystemUsers(SystemUserId),
    CONSTRAINT FK_SystemUsers_UpdatedBy FOREIGN KEY (UpdatedBy) REFERENCES SystemUsers(SystemUserId)
);

INSERT INTO SystemUsers (SystemUserId, UserType, Name, Description, Status, CreatedBy, UpdatedBy)
VALUES ('00000000-0000-0000-0000-000000000001', 'SYSTEM', 'System', 'Default system user', 'Active', '00000000-0000-0000-0000-000000000001', '00000000-0000-0000-0000-000000000001');


CREATE TABLE Users (
    UserId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    MobileNumber NVARCHAR(15) NOT NULL,
    Email NVARCHAR(255),
    PasswordHash NVARCHAR(255) NOT NULL,
    FirstName NVARCHAR(100),
    LastName NVARCHAR(100),
    DateOfBirth DATE,
    Status NVARCHAR(20) NOT NULL, -- Active, Inactive, Suspended, Blocked
    KYCStatus NVARCHAR(20) NOT NULL, -- Pending, Verified, Rejected
    IsDeleted BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy UNIQUEIDENTIFIER NOT NULL, -- References SystemUsers
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedBy UNIQUEIDENTIFIER NOT NULL, -- References SystemUsers
    LastLoginAt DATETIME2,
    CONSTRAINT UQ_Users_MobileNumber UNIQUE (MobileNumber),
    CONSTRAINT UQ_Users_Email UNIQUE (Email),
    CONSTRAINT FK_Users_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES SystemUsers(SystemUserId),
    CONSTRAINT FK_Users_UpdatedBy FOREIGN KEY (UpdatedBy) REFERENCES SystemUsers(SystemUserId)
);

ALTER TABLE Users
DROP COLUMN PasswordHash;

select * from Users;
select * from UserKYC;
CREATE TABLE UserKYC (
    KYCId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL,
    DocumentType NVARCHAR(20) NOT NULL, -- Aadhaar, PAN
    DocumentNumber NVARCHAR(50) NOT NULL,
    DocumentHash NVARCHAR(255) NOT NULL, -- Encrypted document data
    VerificationStatus NVARCHAR(20) NOT NULL,
    VerificationRemarks NVARCHAR(255),
    RetryCount INT NOT NULL DEFAULT 0,
    LastAttemptAt DATETIME2,
    MaxRetries INT NOT NULL DEFAULT 3,
    IsDeleted BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy UNIQUEIDENTIFIER NOT NULL,
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedBy UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT FK_UserKYC_Users FOREIGN KEY (UserId) REFERENCES Users(UserId),
    CONSTRAINT FK_UserKYC_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES SystemUsers(SystemUserId),
    CONSTRAINT FK_UserKYC_UpdatedBy FOREIGN KEY (UpdatedBy) REFERENCES SystemUsers(SystemUserId)
);
ALTER TABLE UserKYC ADD DocumentFilePath NVARCHAR(500) NULL;

CREATE TABLE UserSessions (
    SessionId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL,
    Token NVARCHAR(MAX) NOT NULL,
    RefreshToken NVARCHAR(MAX),
    DeviceId NVARCHAR(100),
    IPAddress NVARCHAR(45),
    UserAgent NVARCHAR(MAX),
    LoginType NVARCHAR(20) NOT NULL, -- OTP, Password
    Status NVARCHAR(20) NOT NULL, -- Active, Expired, Revoked
    ExpiresAt DATETIME2 NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy UNIQUEIDENTIFIER NOT NULL,
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedBy UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT FK_UserSessions_Users FOREIGN KEY (UserId) REFERENCES Users(UserId),
    CONSTRAINT FK_UserSessions_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES SystemUsers(SystemUserId),
    CONSTRAINT FK_UserSessions_UpdatedBy FOREIGN KEY (UpdatedBy) REFERENCES SystemUsers(SystemUserId)
);


CREATE TABLE OTPVerifications (
    OTPId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL,
    OTPType NVARCHAR(20) NOT NULL, -- Login, Transaction, KYC
    OTPValue NVARCHAR(10) NOT NULL,
    ContactType NVARCHAR(20) NOT NULL, -- Mobile, Email
    ContactValue NVARCHAR(100) NOT NULL,
    Status NVARCHAR(20) NOT NULL, -- Pending, Verified, Expired
    ExpiresAt DATETIME2 NOT NULL,
    VerifiedAt DATETIME2,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy UNIQUEIDENTIFIER NOT NULL,
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedBy UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT FK_OTPVerifications_Users FOREIGN KEY (UserId) REFERENCES Users(UserId),
    CONSTRAINT FK_OTPVerifications_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES SystemUsers(SystemUserId),
    CONSTRAINT FK_OTPVerifications_UpdatedBy FOREIGN KEY (UpdatedBy) REFERENCES SystemUsers(SystemUserId)
);

CREATE TABLE QRCodeDetails (
    QRCodeId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL,
    QRCodeType NVARCHAR(20) NOT NULL, -- Static, Dynamic
    Amount DECIMAL(15,2), -- For dynamic QR codes
    Purpose NVARCHAR(100),
    QRCodeData NVARCHAR(MAX) NOT NULL,
    Status NVARCHAR(20) NOT NULL, -- Active, Inactive
    ExpiresAt DATETIME2, -- For dynamic QR codes
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy UNIQUEIDENTIFIER NOT NULL,
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedBy UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT FK_QRCodeDetails_Users FOREIGN KEY (UserId) REFERENCES Users(UserId),
    CONSTRAINT FK_QRCodeDetails_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES SystemUsers(SystemUserId),
    CONSTRAINT FK_QRCodeDetails_UpdatedBy FOREIGN KEY (UpdatedBy) REFERENCES SystemUsers(SystemUserId)
);

CREATE TABLE BankAccounts (
    AccountId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL,
    AccountNumber NVARCHAR(50) NOT NULL,
    IFSC NVARCHAR(11) NOT NULL,
    BankName NVARCHAR(100) NOT NULL,
    AccountType NVARCHAR(20) NOT NULL, -- Savings, Current
    Status NVARCHAR(20) NOT NULL, -- Active, Inactive
    IsDefault BIT DEFAULT 0,
    IsDeleted BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy UNIQUEIDENTIFIER NOT NULL, -- References SystemUsers
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedBy UNIQUEIDENTIFIER NOT NULL, -- References SystemUsers
    CONSTRAINT FK_BankAccounts_Users FOREIGN KEY (UserId) REFERENCES Users(UserId),
    CONSTRAINT FK_BankAccounts_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES SystemUsers(SystemUserId),
    CONSTRAINT FK_BankAccounts_UpdatedBy FOREIGN KEY (UpdatedBy) REFERENCES SystemUsers(SystemUserId)
);

CREATE TABLE VirtualPaymentAddresses (
    VPAId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL,
    VPA NVARCHAR(100) NOT NULL,
    Status NVARCHAR(20) NOT NULL, -- Active, Inactive, Blocked
    IsDefault BIT DEFAULT 0,
    IsDeleted BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy UNIQUEIDENTIFIER NOT NULL, -- References SystemUsers
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedBy UNIQUEIDENTIFIER NOT NULL, -- References SystemUsers
    CONSTRAINT FK_VPA_Users FOREIGN KEY (UserId) REFERENCES Users(UserId),
    CONSTRAINT FK_VPA_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES SystemUsers(SystemUserId),
    CONSTRAINT FK_VPA_UpdatedBy FOREIGN KEY (UpdatedBy) REFERENCES SystemUsers(SystemUserId),
    CONSTRAINT UQ_VPA_Address UNIQUE (VPA)
);

CREATE TABLE Transactions (
    TransactionId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL,
    TransactionType NVARCHAR(20) NOT NULL, -- Send, Request, Refund
    Amount DECIMAL(15,2) NOT NULL,
    Currency NVARCHAR(3) DEFAULT 'INR',
    Status NVARCHAR(20) NOT NULL, -- Pending, Success, Failed, Reversed
    SourceType NVARCHAR(20) NOT NULL, -- VPA, Account, Mobile
    SourceIdentifier NVARCHAR(100) NOT NULL,
    DestinationType NVARCHAR(20) NOT NULL, -- VPA, Account, Mobile
    DestinationIdentifier NVARCHAR(100) NOT NULL,
    ReferenceId NVARCHAR(100),
    NPCIReferenceId NVARCHAR(100),
    Remarks NVARCHAR(255),
    IsRefund BIT DEFAULT 0,
    OriginalTransactionId UNIQUEIDENTIFIER, -- For refunds
    QRCodeId UNIQUEIDENTIFIER, -- For QR code transactions
    IsDeleted BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy UNIQUEIDENTIFIER NOT NULL,
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedBy UNIQUEIDENTIFIER NOT NULL,
    CompletedAt DATETIME2,
    CONSTRAINT FK_Transactions_Users FOREIGN KEY (UserId) REFERENCES Users(UserId),
    CONSTRAINT FK_Transactions_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES SystemUsers(SystemUserId),
    CONSTRAINT FK_Transactions_UpdatedBy FOREIGN KEY (UpdatedBy) REFERENCES SystemUsers(SystemUserId),
    CONSTRAINT FK_Transactions_OriginalTransaction FOREIGN KEY (OriginalTransactionId) REFERENCES Transactions(TransactionId),
    CONSTRAINT FK_Transactions_QRCode FOREIGN KEY (QRCodeId) REFERENCES QRCodeDetails(QRCodeId),
    CONSTRAINT UQ_Transactions_ReferenceId UNIQUE (ReferenceId)
);

CREATE TABLE TransactionLogs (
    LogId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    TransactionId UNIQUEIDENTIFIER NOT NULL,
    Status NVARCHAR(20) NOT NULL,
    Remarks NVARCHAR(255),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy UNIQUEIDENTIFIER NOT NULL, -- References SystemUsers
    CONSTRAINT FK_TransactionLogs_Transactions FOREIGN KEY (TransactionId) REFERENCES Transactions(TransactionId),
    CONSTRAINT FK_TransactionLogs_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES SystemUsers(SystemUserId)
);

CREATE TABLE UPIMandates (
    MandateId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL,
    Amount DECIMAL(15,2) NOT NULL,
    Frequency NVARCHAR(20) NOT NULL, -- Daily, Weekly, Monthly
    StartDate DATE NOT NULL,
    EndDate DATE,
    Status NVARCHAR(20) NOT NULL, -- Active, Completed, Cancelled
    ReferenceId NVARCHAR(100),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy UNIQUEIDENTIFIER NOT NULL, -- References SystemUsers
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedBy UNIQUEIDENTIFIER NOT NULL, -- References SystemUsers
    CONSTRAINT FK_UPIMandates_Users FOREIGN KEY (UserId) REFERENCES Users(UserId),
    CONSTRAINT FK_UPIMandates_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES SystemUsers(SystemUserId),
    CONSTRAINT FK_UPIMandates_UpdatedBy FOREIGN KEY (UpdatedBy) REFERENCES SystemUsers(SystemUserId),
    CONSTRAINT UQ_UPIMandates_ReferenceId UNIQUE (ReferenceId)
);

CREATE TABLE Notifications (
    NotificationId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL,
    Type NVARCHAR(50) NOT NULL, -- Transaction, Security, System
    Channel NVARCHAR(20) NOT NULL, -- SMS, Email, Push
    Status NVARCHAR(20) NOT NULL, -- Pending, Sent, Failed
    Content NVARCHAR(MAX) NOT NULL,
    TemplateId NVARCHAR(100), -- For templated notifications
    TemplateParameters NVARCHAR(MAX), -- JSON parameters for template
    RetryCount INT DEFAULT 0,
    LastRetryAt DATETIME2,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy UNIQUEIDENTIFIER NOT NULL,
    SentAt DATETIME2,
    CONSTRAINT FK_Notifications_Users FOREIGN KEY (UserId) REFERENCES Users(UserId),
    CONSTRAINT FK_Notifications_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES SystemUsers(SystemUserId)
);

CREATE TABLE AuditLogs (
    LogId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL,
    Action NVARCHAR(100) NOT NULL,
    EntityType NVARCHAR(50) NOT NULL,
    EntityId UNIQUEIDENTIFIER NOT NULL,
    OldValue NVARCHAR(MAX),
    NewValue NVARCHAR(MAX),
    IPAddress NVARCHAR(45),
    UserAgent NVARCHAR(MAX),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy UNIQUEIDENTIFIER NOT NULL, -- References SystemUsers
    CONSTRAINT FK_AuditLogs_Users FOREIGN KEY (UserId) REFERENCES Users(UserId),
    CONSTRAINT FK_AuditLogs_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES SystemUsers(SystemUserId)
);


## Indexes

```sql
-- Users Table
CREATE NONCLUSTERED INDEX IX_Users_MobileNumber ON Users(MobileNumber);
CREATE NONCLUSTERED INDEX IX_Users_Email ON Users(Email);
CREATE NONCLUSTERED INDEX IX_Users_Status ON Users(Status);

-- Transactions Table
CREATE NONCLUSTERED INDEX IX_Transactions_UserId ON Transactions(UserId);
CREATE NONCLUSTERED INDEX IX_Transactions_Status ON Transactions(Status);
CREATE NONCLUSTERED INDEX IX_Transactions_CreatedAt ON Transactions(CreatedAt);
CREATE NONCLUSTERED INDEX IX_Transactions_ReferenceId ON Transactions(ReferenceId);

-- UPI Mandates Table
CREATE NONCLUSTERED INDEX IX_UPIMandates_UserId ON UPIMandates(UserId);
CREATE NONCLUSTERED INDEX IX_UPIMandates_Status ON UPIMandates(Status);

-- Audit Logs Table
CREATE NONCLUSTERED INDEX IX_AuditLogs_UserId ON AuditLogs(UserId);
CREATE NONCLUSTERED INDEX IX_AuditLogs_CreatedAt ON AuditLogs(CreatedAt);

-- UserSessions Table
CREATE NONCLUSTERED INDEX IX_UserSessions_UserId ON UserSessions(UserId);

CREATE NONCLUSTERED INDEX IX_UserSessions_Status ON UserSessions(Status);

-- OTPVerifications Table
CREATE NONCLUSTERED INDEX IX_OTPVerifications_UserId ON OTPVerifications(UserId);
CREATE NONCLUSTERED INDEX IX_OTPVerifications_Status ON OTPVerifications(Status);
CREATE NONCLUSTERED INDEX IX_OTPVerifications_ContactValue ON OTPVerifications(ContactValue);

-- QRCodeDetails Table
CREATE NONCLUSTERED INDEX IX_QRCodeDetails_UserId ON QRCodeDetails(UserId);
CREATE NONCLUSTERED INDEX IX_QRCodeDetails_Status ON QRCodeDetails(Status);

-- Transactions Table (Additional)
CREATE NONCLUSTERED INDEX IX_Transactions_QRCodeId ON Transactions(QRCodeId);
CREATE NONCLUSTERED INDEX IX_Transactions_OriginalTransactionId ON Transactions(OriginalTransactionId);

-- Add indexes for soft delete queries
CREATE NONCLUSTERED INDEX IX_Users_IsDeleted ON Users(IsDeleted);
CREATE NONCLUSTERED INDEX IX_BankAccounts_IsDeleted ON BankAccounts(IsDeleted);
CREATE NONCLUSTERED INDEX IX_Transactions_IsDeleted ON Transactions(IsDeleted);
CREATE NONCLUSTERED INDEX IX_UserKYC_IsDeleted ON UserKYC(IsDeleted);
CREATE NONCLUSTERED INDEX IX_VPA_IsDeleted ON VirtualPaymentAddresses(IsDeleted);

-- Add index for KYC retry tracking
CREATE NONCLUSTERED INDEX IX_UserKYC_RetryCount ON UserKYC(RetryCount, LastAttemptAt);