using Xabe.FFmpeg;

namespace Tiktok.ScheduledJob.Services.Interfaces;

public interface IVideoUploadHandlerService
{
    Task ExtractAudio(string videoName, string audioName);
    
    Task ReplaceAudio(string videoName, string audioName, string newVideoName, IMediaInfo? audioInfo = null, IMediaInfo? videoInfo = null);
    
    Task<IMediaInfo> GenerateThumbnail(string videoName, IMediaInfo? videoInfo = null);

    Task<IMediaInfo> GetVideoMediaInfoFromStorage(string videoName);

    Task<IMediaInfo> GetAudioMediaInfoFromStorage(string audioName);

    Task DeleteVideoAsync(string videoName);
    Task DeleteAudioAsync(string audioName);
    Task DeleteThumbnailAsync(string thumbnailName);
}