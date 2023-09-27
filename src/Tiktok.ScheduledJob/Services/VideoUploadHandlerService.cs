using Tiktok.API.Domain.Configurations;
using Tiktok.ScheduledJob.Services.Interfaces;
using Xabe.FFmpeg;

namespace Tiktok.ScheduledJob.Services;

public class VideoUploadHandlerService : IVideoUploadHandlerService
{
    private readonly DiskStorageSettings _diskStorageSettings;

    public VideoUploadHandlerService(DiskStorageSettings diskStorageSettings)
    {
        _diskStorageSettings = diskStorageSettings;
    }

    public async Task ExtractAudio(string videoName, string audioName)
    {
        var videoPath = Path.Combine(_diskStorageSettings.StoragePath, DiskStorageSettings.Video);
        var audioPath = Path.Combine(_diskStorageSettings.StoragePath, DiskStorageSettings.Audio);
        
        var videoInput = Path.Combine(videoPath, videoName);
        var audioOutput = Path.Combine(audioPath, audioName);
        IConversion conversion = await FFmpeg.Conversions.FromSnippet.ExtractAudio(videoInput, audioOutput);
        await conversion.Start();
    }

    public async Task ReplaceAudio(string videoName, string audioName, string newVideoName, IMediaInfo? audioInfo = null, IMediaInfo? videoInfo = null)
    {
        var audioPath = Path.Combine(_diskStorageSettings.StoragePath, DiskStorageSettings.Audio);
        var videoPath = Path.Combine(_diskStorageSettings.StoragePath, DiskStorageSettings.Video);
        
        var videoInput = Path.Combine(videoPath, videoName);
        var audioInput = Path.Combine(audioPath, audioName);

        audioInfo ??= await GetAudioMediaInfoFromStorage(audioName);
        videoInfo ??= await GetVideoMediaInfoFromStorage(videoName);
        
        var checkShortest = audioInfo.Duration > videoInfo.Duration;
        
        var newVideoOutput = Path.Combine(videoPath, newVideoName);
        
        var conversionResult = FFmpeg.Conversions.New()
            .AddParameter(
                $"-i {videoInput} -i {audioInput} -map 0:v -map 1:a -c:v copy {(checkShortest ? " -shortest" : "")}")
            .SetOutput(newVideoOutput);
        
        await conversionResult.Start();
    }

    public async Task<IMediaInfo> GenerateThumbnail(string videoName, IMediaInfo? videoInfo = null)
    {
        var thumbnailPath = Path.Combine(_diskStorageSettings.StoragePath, DiskStorageSettings.Thumbnail);
        
        var videoNameExcludeExt = videoName.Split(".")[0];

        videoInfo ??= await GetVideoMediaInfoFromStorage(videoName);
        var videoStream = videoInfo.VideoStreams.First()?.SetCodec(VideoCodec.png);
        
        await FFmpeg.Conversions.New()
            .AddStream(videoStream)
            .ExtractNthFrame(3, (number) => Path.Combine(thumbnailPath, $"{videoNameExcludeExt}.png"))
            .Start();
        
        return videoInfo;
    }

    public async Task<IMediaInfo> GetVideoMediaInfoFromStorage(string videoName)
    {
        var videoPath = Path.Combine(_diskStorageSettings.StoragePath, DiskStorageSettings.Video);
        var videoInput = Path.Combine(videoPath, videoName);

        return await FFmpeg.GetMediaInfo(videoInput);
    }
    
    public async Task<IMediaInfo> GetAudioMediaInfoFromStorage(string audioName)
    {
        var audioPath = Path.Combine(_diskStorageSettings.StoragePath, DiskStorageSettings.Audio);
        var audioInput = Path.Combine(audioPath, audioName);

        return await FFmpeg.GetMediaInfo(audioInput);
    }
    
    public async Task DeleteVideoAsync(string videoName)
    {
        var videoPath = Path.Combine(_diskStorageSettings.StoragePath, DiskStorageSettings.Video);
        var videoInput = Path.Combine(videoPath, videoName);

        // convert to async
        if (System.IO.File.Exists(videoInput))
        {
            await Task.Run(() => System.IO.File.Delete(videoInput));
        }
    }
    
    public async Task DeleteAudioAsync(string audioName)
    {
        var audioPath = Path.Combine(_diskStorageSettings.StoragePath, DiskStorageSettings.Video);
        var audioInput = Path.Combine(audioPath, audioName);

        // convert to async
        if (System.IO.File.Exists(audioInput))
        {
            await Task.Run(() => System.IO.File.Delete(audioInput));
        }
    }
    
    public async Task DeleteThumbnailAsync(string thumbnailName)
    {
        var thumbnailPath = Path.Combine(_diskStorageSettings.StoragePath, DiskStorageSettings.Thumbnail);
        var thumbnailInput = Path.Combine(thumbnailPath, thumbnailName);

        // convert to async
        if (System.IO.File.Exists(thumbnailInput))
        {
            await Task.Run(() => System.IO.File.Delete(thumbnailInput));
        }
    }
}