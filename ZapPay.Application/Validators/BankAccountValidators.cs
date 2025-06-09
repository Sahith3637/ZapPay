using FluentValidation;
using ZapPay.Application.DTOs;

namespace ZapPay.Application.Validators;

public class CreateBankAccountDtoValidator : AbstractValidator<CreateBankAccountDto>
{
    public CreateBankAccountDtoValidator()
    {
        RuleFor(x => x.AccountNumber)
            .NotEmpty().WithMessage("Account number is required")
            .Length(9, 18).WithMessage("Account number must be between 9 and 18 digits")
            .Matches(@"^\d+$").WithMessage("Account number must contain only digits");

        RuleFor(x => x.IFSC)
            .NotEmpty().WithMessage("IFSC code is required")
            .Length(11).WithMessage("IFSC code must be exactly 11 characters")
            .Matches(@"^[A-Z]{4}0[A-Z0-9]{6}$").WithMessage("Invalid IFSC code format (e.g., SBIN0001234)");

        RuleFor(x => x.BankName)
            .NotEmpty().WithMessage("Bank name is required")
            .MaximumLength(100).WithMessage("Bank name cannot exceed 100 characters");

        RuleFor(x => x.AccountType)
            .NotEmpty().WithMessage("Account type is required")
            .Must(x => x == "Savings" || x == "Current")
            .WithMessage("Account type must be either 'Savings' or 'Current'");

        RuleFor(x => x.InitialBalance)
            .GreaterThanOrEqualTo(0).WithMessage("Initial balance cannot be negative")
            .LessThanOrEqualTo(1000000000).WithMessage("Initial balance cannot exceed 1 billion");
    }
}

public class BankAccountResponseDtoValidator : AbstractValidator<BankAccountResponseDto>
{
    public BankAccountResponseDtoValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty().WithMessage("Account ID is required");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");

        RuleFor(x => x.AccountNumber)
            .NotEmpty().WithMessage("Account number is required")
            .Length(9, 18).WithMessage("Account number must be between 9 and 18 digits")
            .Matches(@"^\d+$").WithMessage("Account number must contain only digits");

        RuleFor(x => x.IFSC)
            .NotEmpty().WithMessage("IFSC code is required")
            .Length(11).WithMessage("IFSC code must be exactly 11 characters")
            .Matches(@"^[A-Z]{4}0[A-Z0-9]{6}$").WithMessage("Invalid IFSC code format (e.g., SBIN0001234)");

        RuleFor(x => x.BankName)
            .NotEmpty().WithMessage("Bank name is required")
            .MaximumLength(100).WithMessage("Bank name cannot exceed 100 characters");

        RuleFor(x => x.AccountType)
            .NotEmpty().WithMessage("Account type is required")
            .Must(x => x == "Savings" || x == "Current")
            .WithMessage("Account type must be either 'Savings' or 'Current'");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required")
            .Must(x => x == "Active" || x == "Inactive")
            .WithMessage("Status must be either 'Active' or 'Inactive'");

        RuleFor(x => x.Balance)
            .GreaterThanOrEqualTo(0).WithMessage("Balance cannot be negative")
            .LessThanOrEqualTo(1000000000).WithMessage("Balance cannot exceed 1 billion");

        RuleFor(x => x.CreatedAt)
            .NotEmpty().WithMessage("Created date is required")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Created date cannot be in the future");
    }
} 