using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZapPay.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDocumentNumberLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SystemUsers",
                columns: table => new
                {
                    SystemUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    UserType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getutcdate())"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getutcdate())"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SystemUs__8788C295745FB76A", x => x.SystemUserId);
                    table.ForeignKey(
                        name: "FK_SystemUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "SystemUsers",
                        principalColumn: "SystemUserId");
                    table.ForeignKey(
                        name: "FK_SystemUsers_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "SystemUsers",
                        principalColumn: "SystemUserId");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    MobileNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    KYCStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getutcdate())"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getutcdate())"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Users__1788CC4CA8092BC3", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "SystemUsers",
                        principalColumn: "SystemUserId");
                    table.ForeignKey(
                        name: "FK_Users_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "SystemUsers",
                        principalColumn: "SystemUserId");
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    LogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EntityType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OldValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IPAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getutcdate())"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__AuditLog__5E548648E986D3DC", x => x.LogId);
                    table.ForeignKey(
                        name: "FK_AuditLogs_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "SystemUsers",
                        principalColumn: "SystemUserId");
                    table.ForeignKey(
                        name: "FK_AuditLogs_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "BankAccounts",
                columns: table => new
                {
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IFSC = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    BankName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AccountType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getutcdate())"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getutcdate())"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__BankAcco__349DA5A6ED6875B1", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_BankAccounts_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "SystemUsers",
                        principalColumn: "SystemUserId");
                    table.ForeignKey(
                        name: "FK_BankAccounts_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "SystemUsers",
                        principalColumn: "SystemUserId");
                    table.ForeignKey(
                        name: "FK_BankAccounts_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotificationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Channel = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TemplateId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TemplateParameters = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RetryCount = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    LastRetryAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getutcdate())"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Notifica__20CF2E129A8EB024", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_Notifications_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "SystemUsers",
                        principalColumn: "SystemUserId");
                    table.ForeignKey(
                        name: "FK_Notifications_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "OTPVerifications",
                columns: table => new
                {
                    OTPId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OTPType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    OTPValue = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ContactType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ContactValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getutcdate())"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getutcdate())"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__OTPVerif__5C2EC4823CCCCC83", x => x.OTPId);
                    table.ForeignKey(
                        name: "FK_OTPVerifications_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "SystemUsers",
                        principalColumn: "SystemUserId");
                    table.ForeignKey(
                        name: "FK_OTPVerifications_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "SystemUsers",
                        principalColumn: "SystemUserId");
                    table.ForeignKey(
                        name: "FK_OTPVerifications_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "QRCodeDetails",
                columns: table => new
                {
                    QRCodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QRCodeType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(15,2)", nullable: true),
                    Purpose = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    QRCodeData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getutcdate())"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getutcdate())"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__QRCodeDe__62FECD92773FD50C", x => x.QRCodeId);
                    table.ForeignKey(
                        name: "FK_QRCodeDetails_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "SystemUsers",
                        principalColumn: "SystemUserId");
                    table.ForeignKey(
                        name: "FK_QRCodeDetails_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "SystemUsers",
                        principalColumn: "SystemUserId");
                    table.ForeignKey(
                        name: "FK_QRCodeDetails_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "UPIMandates",
                columns: table => new
                {
                    MandateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(15,2)", nullable: false),
                    Frequency = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ReferenceId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getutcdate())"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getutcdate())"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__UPIManda__A5A5EC654A41F8B0", x => x.MandateId);
                    table.ForeignKey(
                        name: "FK_UPIMandates_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "SystemUsers",
                        principalColumn: "SystemUserId");
                    table.ForeignKey(
                        name: "FK_UPIMandates_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "SystemUsers",
                        principalColumn: "SystemUserId");
                    table.ForeignKey(
                        name: "FK_UPIMandates_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "UserKYC",
                columns: table => new
                {
                    KYCId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DocumentNumber = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    DocumentHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    VerificationStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    VerificationRemarks = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    RetryCount = table.Column<int>(type: "int", nullable: false),
                    LastAttemptAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MaxRetries = table.Column<int>(type: "int", nullable: false, defaultValue: 3),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getutcdate())"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getutcdate())"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentFilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__UserKYC__4ED3C4C9FC2C450E", x => x.KYCId);
                    table.ForeignKey(
                        name: "FK_UserKYC_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "SystemUsers",
                        principalColumn: "SystemUserId");
                    table.ForeignKey(
                        name: "FK_UserKYC_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "SystemUsers",
                        principalColumn: "SystemUserId");
                    table.ForeignKey(
                        name: "FK_UserKYC_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "UserSessions",
                columns: table => new
                {
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IPAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LoginType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getutcdate())"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getutcdate())"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__UserSess__C9F49290E487B185", x => x.SessionId);
                    table.ForeignKey(
                        name: "FK_UserSessions_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "SystemUsers",
                        principalColumn: "SystemUserId");
                    table.ForeignKey(
                        name: "FK_UserSessions_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "SystemUsers",
                        principalColumn: "SystemUserId");
                    table.ForeignKey(
                        name: "FK_UserSessions_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "VirtualPaymentAddresses",
                columns: table => new
                {
                    VPAId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VPA = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getutcdate())"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getutcdate())"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__VirtualP__3A2D0D5BBFF2D802", x => x.VPAId);
                    table.ForeignKey(
                        name: "FK_VPA_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "SystemUsers",
                        principalColumn: "SystemUserId");
                    table.ForeignKey(
                        name: "FK_VPA_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "SystemUsers",
                        principalColumn: "SystemUserId");
                    table.ForeignKey(
                        name: "FK_VPA_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(15,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true, defaultValue: "INR"),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SourceType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SourceIdentifier = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DestinationType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DestinationIdentifier = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ReferenceId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NPCIReferenceId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsRefund = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    OriginalTransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    QRCodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getutcdate())"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getutcdate())"),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Transact__55433A6BCCAF6897", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_Transactions_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "SystemUsers",
                        principalColumn: "SystemUserId");
                    table.ForeignKey(
                        name: "FK_Transactions_OriginalTransaction",
                        column: x => x.OriginalTransactionId,
                        principalTable: "Transactions",
                        principalColumn: "TransactionId");
                    table.ForeignKey(
                        name: "FK_Transactions_QRCode",
                        column: x => x.QRCodeId,
                        principalTable: "QRCodeDetails",
                        principalColumn: "QRCodeId");
                    table.ForeignKey(
                        name: "FK_Transactions_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "SystemUsers",
                        principalColumn: "SystemUserId");
                    table.ForeignKey(
                        name: "FK_Transactions_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "TransactionLogs",
                columns: table => new
                {
                    LogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    TransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "(getutcdate())"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Transact__5E548648EA8E8B75", x => x.LogId);
                    table.ForeignKey(
                        name: "FK_TransactionLogs_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "SystemUsers",
                        principalColumn: "SystemUserId");
                    table.ForeignKey(
                        name: "FK_TransactionLogs_Transactions",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "TransactionId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_CreatedAt",
                table: "AuditLogs",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_CreatedBy",
                table: "AuditLogs",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_UserId",
                table: "AuditLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_CreatedBy",
                table: "BankAccounts",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_IsDeleted",
                table: "BankAccounts",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_UpdatedBy",
                table: "BankAccounts",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_UserId",
                table: "BankAccounts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CreatedBy",
                table: "Notifications",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OTPVerifications_ContactValue",
                table: "OTPVerifications",
                column: "ContactValue");

            migrationBuilder.CreateIndex(
                name: "IX_OTPVerifications_CreatedBy",
                table: "OTPVerifications",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_OTPVerifications_Status",
                table: "OTPVerifications",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_OTPVerifications_UpdatedBy",
                table: "OTPVerifications",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_OTPVerifications_UserId",
                table: "OTPVerifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_QRCodeDetails_CreatedBy",
                table: "QRCodeDetails",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_QRCodeDetails_Status",
                table: "QRCodeDetails",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_QRCodeDetails_UpdatedBy",
                table: "QRCodeDetails",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_QRCodeDetails_UserId",
                table: "QRCodeDetails",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemUsers_CreatedBy",
                table: "SystemUsers",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SystemUsers_UpdatedBy",
                table: "SystemUsers",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionLogs_CreatedBy",
                table: "TransactionLogs",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionLogs_TransactionId",
                table: "TransactionLogs",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CreatedAt",
                table: "Transactions",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CreatedBy",
                table: "Transactions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_IsDeleted",
                table: "Transactions",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_OriginalTransactionId",
                table: "Transactions",
                column: "OriginalTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_QRCodeId",
                table: "Transactions",
                column: "QRCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ReferenceId",
                table: "Transactions",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_Status",
                table: "Transactions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_UpdatedBy",
                table: "Transactions",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_UserId",
                table: "Transactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "UQ_Transactions_ReferenceId",
                table: "Transactions",
                column: "ReferenceId",
                unique: true,
                filter: "[ReferenceId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UPIMandates_CreatedBy",
                table: "UPIMandates",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UPIMandates_Status",
                table: "UPIMandates",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_UPIMandates_UpdatedBy",
                table: "UPIMandates",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UPIMandates_UserId",
                table: "UPIMandates",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "UQ_UPIMandates_ReferenceId",
                table: "UPIMandates",
                column: "ReferenceId",
                unique: true,
                filter: "[ReferenceId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserKYC_CreatedBy",
                table: "UserKYC",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserKYC_IsDeleted",
                table: "UserKYC",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_UserKYC_RetryCount",
                table: "UserKYC",
                columns: new[] { "RetryCount", "LastAttemptAt" });

            migrationBuilder.CreateIndex(
                name: "IX_UserKYC_UpdatedBy",
                table: "UserKYC",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserKYC_UserId",
                table: "UserKYC",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatedBy",
                table: "Users",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IsDeleted",
                table: "Users",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Users_MobileNumber",
                table: "Users",
                column: "MobileNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Status",
                table: "Users",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UpdatedBy",
                table: "Users",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "UQ_Users_Email",
                table: "Users",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ_Users_MobileNumber",
                table: "Users",
                column: "MobileNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSessions_CreatedBy",
                table: "UserSessions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserSessions_Status",
                table: "UserSessions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_UserSessions_UpdatedBy",
                table: "UserSessions",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserSessions_UserId",
                table: "UserSessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_VirtualPaymentAddresses_CreatedBy",
                table: "VirtualPaymentAddresses",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_VirtualPaymentAddresses_UpdatedBy",
                table: "VirtualPaymentAddresses",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_VirtualPaymentAddresses_UserId",
                table: "VirtualPaymentAddresses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_VPA_IsDeleted",
                table: "VirtualPaymentAddresses",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "UQ_VPA_Address",
                table: "VirtualPaymentAddresses",
                column: "VPA",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "BankAccounts");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "OTPVerifications");

            migrationBuilder.DropTable(
                name: "TransactionLogs");

            migrationBuilder.DropTable(
                name: "UPIMandates");

            migrationBuilder.DropTable(
                name: "UserKYC");

            migrationBuilder.DropTable(
                name: "UserSessions");

            migrationBuilder.DropTable(
                name: "VirtualPaymentAddresses");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "QRCodeDetails");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "SystemUsers");
        }
    }
}
