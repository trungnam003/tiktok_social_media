using Microsoft.AspNetCore.Http;

namespace Tiktok.API.Domain.Services;

public interface IFileService
{
    Task<string> SaveVideoToStorageAsync(IFormFile file, string fileName, string? folderName = null);
}