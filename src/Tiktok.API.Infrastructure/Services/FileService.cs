
using Microsoft.AspNetCore.Http;
using Tiktok.API.Domain.Configurations;
using Tiktok.API.Domain.Exceptions;
using Tiktok.API.Domain.Services;
using Tiktok.API.Infrastructure.Helper;

namespace Tiktok.API.Infrastructure.Services;

public class FileService : IFileService
{
    private readonly DiskStorageSettings _diskStorageSettings;

    public FileService(DiskStorageSettings diskStorageSettings)
    {
        _diskStorageSettings = diskStorageSettings;
    }

    public async Task<string> SaveVideoToStorageAsync(IFormFile file, string fileName, string? folderName = null)
    {
        var storagePath = _diskStorageSettings.StoragePath.TrimEnd('\\').TrimEnd('/');
        var folderPath = Path.Combine(storagePath, folderName ?? DiskStorageSettings.Video);

        await using var f = file.OpenReadStream();
        var isValid = FileHelpers.IsValidFileExtensionAndSignature(fileName, f, new[] { ".mp4" });

        if (!isValid) throw new BadRequestException("Video upload not supported");

        await using var targetStream =
            System.IO.File.Create(Path.Combine(folderPath, fileName));

        await file.CopyToAsync(targetStream);
        await f.DisposeAsync();
        await targetStream.DisposeAsync();

        return fileName;
    }

    public Task<bool> CheckFileExistAsync(string fileName, string folderName)
    {
        var storagePath = _diskStorageSettings.StoragePath.TrimEnd('\\').TrimEnd('/');
        var folderPath = Path.Combine(storagePath, folderName);

        return Task.FromResult(System.IO.File.Exists(Path.Combine(folderPath, fileName)));
    }

    public Task<string> GetFilePathAsync(string fileName, string folderName)
    {
        var storagePath = _diskStorageSettings.StoragePath.TrimEnd('\\').TrimEnd('/');
        var folderPath = Path.Combine(storagePath, folderName);

        return Task.FromResult(Path.Combine(folderPath, fileName));
    }
}