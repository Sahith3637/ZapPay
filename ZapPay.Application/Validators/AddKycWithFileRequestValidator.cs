using FluentValidation;
using ZapPay.Application.DTOs;
using Microsoft.AspNetCore.Http;

namespace ZapPay.Application.Validators;

public class AddKycWithFileRequestValidator : AbstractValidator<AddKycWithFileRequestDto>
{
    public AddKycWithFileRequestValidator()
    {
        RuleFor(x => x.DocumentType)
            .NotEmpty()
            .MaximumLength(20)
            .Must(t => new[] { "Aadhaar", "PAN" }.Contains(t))
            .WithMessage("Document type must be either Aadhaar or PAN");

        RuleFor(x => x.DocumentNumber)
            .NotEmpty()
            .MaximumLength(50)
            .Must((dto, value) =>
            {
                if (dto.DocumentType == "Aadhaar")
                    return System.Text.RegularExpressions.Regex.IsMatch(value, @"^\d{12}$");
                if (dto.DocumentType == "PAN")
                    return System.Text.RegularExpressions.Regex.IsMatch(value, @"^[A-Z]{5}[0-9]{4}[A-Z]{1}$");
                return false;
            })
            .WithMessage("Invalid document number format");

        RuleFor(x => x.DocumentFile)
            .NotNull()
            .Must(file => file != null && file.Length > 0)
            .WithMessage("Document file is required.")
            .Must(file =>
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".pdf" };
                var ext = System.IO.Path.GetExtension(file.FileName).ToLowerInvariant();
                return allowedExtensions.Contains(ext);
            })
            .WithMessage("Only JPG, PNG, and PDF files are allowed.")
            .Must(file => file.Length <= 5 * 1024 * 1024) // 5MB limit
            .WithMessage("File size must be 5MB or less.");
    }
} 