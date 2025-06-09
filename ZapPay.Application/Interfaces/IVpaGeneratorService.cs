using System.Threading.Tasks;

namespace ZapPay.Application.Interfaces;

public interface IVpaGeneratorService
{
    Task<string> GenerateUniqueVpaAsync(string mobileOrEmail);
} 