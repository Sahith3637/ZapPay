using Microsoft.AspNetCore.Http;
using ZapPay.Infrastructure.Interfaces;

public class FileStorageService : IFileStorageService
{
    private readonly string _storagePath = "KycDocuments";

    public async Task<string> SaveFileAsync(IFormFile file)
    {
        var fileName = $"{Guid.NewGuid()}_{file.FileName}";
        var filePath = Path.Combine(_storagePath, fileName);

        Directory.CreateDirectory(_storagePath);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return filePath;
    }
} 