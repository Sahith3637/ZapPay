using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ZapPay.Infrastructure.Interfaces;

public interface IFileStorageService
{
    Task<string> SaveFileAsync(IFormFile file);
} 