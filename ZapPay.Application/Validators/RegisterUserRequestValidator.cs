using FluentValidation;
using ZapPay.Application.DTOs;

namespace ZapPay.Application.Validators;

public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequestDto>
{
    public RegisterUserRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(100)
            .Matches("^[a-zA-Z ]*$").WithMessage("First name should only contain letters and spaces");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(100)
            .Matches("^[a-zA-Z ]*$").WithMessage("Last name should only contain letters and spaces");

        RuleFor(x => x.MobileNumber)
            .NotEmpty()
            .Matches(@"^\d{10}$").WithMessage("Mobile number must be 10 digits");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(255);

        RuleFor(x => x.DateOfBirth)
            .NotEmpty()
            .Must(dob => dob.AddYears(18) <= DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("User must be at least 18 years old");
    }
} 