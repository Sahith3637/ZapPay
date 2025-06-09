using System.Text.RegularExpressions;
using ZapPay.Application.Interfaces;
using ZapPay.Persistence.Interfaces;
using Serilog;

namespace ZapPay.Application.Services;

public class VpaGeneratorService : IVpaGeneratorService
{
    private readonly IVpaRepository _vpaRepository;
    private const string BankCode = "upi"; // Change as needed
    private const int MaxAttempts = 5;
    private static readonly Regex ValidVpaRegex = new("^[a-zA-Z0-9_.-]+$", RegexOptions.Compiled);

    public VpaGeneratorService(IVpaRepository vpaRepository)
    {
        _vpaRepository = vpaRepository;
    }

    public async Task<string> GenerateUniqueVpaAsync(string mobileOrEmail)
    {
        string prefix = SanitizePrefix(mobileOrEmail);
        for (int attempt = 0; attempt < MaxAttempts; attempt++)
        {
            string vpa = attempt == 0
                ? $"{prefix}@{BankCode}"
                : $"{prefix}{RandomSuffix()}@{BankCode}";

            if (!await _vpaRepository.VpaExistsAsync(vpa))
            {
                Log.Information("Generated unique VPA: {Vpa}", vpa);
                return vpa;
            }
        }
        Log.Error("Failed to generate unique VPA for {Prefix}", prefix);
        throw new Exception("Could not generate unique VPA after multiple attempts");
    }

    private string SanitizePrefix(string input)
    {
        string prefix = input.Split('@')[0].ToLowerInvariant();
        prefix = Regex.Replace(prefix, "[^a-zA-Z0-9_.-]", "");
        if (string.IsNullOrWhiteSpace(prefix))
            prefix = "user" + DateTime.UtcNow.Ticks;
        return prefix;
    }

    private string RandomSuffix()
    {
        return new Random().Next(100, 999).ToString();
    }
} 