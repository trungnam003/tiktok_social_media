using Contracts.EventBusMessages.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Tiktok.API.Application.Common.Repositories;
using Tiktok.API.Domain.Entities;
using Tiktok.ScheduledJob.Services.Interfaces;
using Xabe.FFmpeg;

namespace Tiktok.ScheduledJob.Consumers;

public class UserUploadVideoConsumer : IConsumer<UserUploadVideoEvent>
{
    private readonly IVideoUploadHandlerService _videoService;
    private readonly IAudioRepository _audioRepository;
    private readonly IVideoRepository _videoRepository;
    public UserUploadVideoConsumer(IVideoUploadHandlerService videoService, IAudioRepository audioRepository, IVideoRepository videoRepository)
    {
        _videoService = videoService;
        _audioRepository = audioRepository;
        _videoRepository = videoRepository;
    }
    
    public async Task Consume(ConsumeContext<UserUploadVideoEvent> context)
    {
        var message = context.Message;
        var videoName = message.UseExternalAudio ? message.VideoName.Replace("_temp", "") : message.VideoName;
        
        var video = await _videoRepository.FindByCondition(x => x.Id == message.VideoId).FirstOrDefaultAsync();
        if (video == null)
        {
            return;
        }
        var videoMediaInfo = await _videoService.GetVideoMediaInfoFromStorage(message.VideoName);

        await _videoService.GenerateThumbnail(videoName, videoMediaInfo);
        
        video.ThumbnailUrl = $"/api/videos/thumbnail/{video.Id}";
        video.MsDuration = (long)videoMediaInfo.Duration.TotalMilliseconds;
        if (message.UseExternalAudio)
        {
            var audio = await _audioRepository.FindByCondition(x => x.Id == message.ExternalAudioId)
                .FirstOrDefaultAsync();
            var audioFileName = $"{audio.Id}.mp3";
            var audioMediaInfo = await _videoService.GetAudioMediaInfoFromStorage(audioFileName);
            await _videoService.ReplaceAudio(message.VideoName, audioFileName, videoName, videoInfo:videoMediaInfo, audioInfo:audioMediaInfo);
            await _videoService.DeleteVideoAsync(message.VideoName);
            video.ExternalAudioId = audio.Id;
        }
        else
        {
            var audioId = Guid.NewGuid().ToString();
            videoName = context.Message.VideoName;
            var audioName = audioId + ".mp3";
        
            await _videoService.ExtractAudio(videoName, audioName);
            
            var result = await Task.WhenAll(_videoService.GetAudioMediaInfoFromStorage(audioName),
                _videoService.GenerateThumbnail(videoName));
            
            var audioInfo = result[0];
            video.ThumbnailUrl = $"/api/videos/thumbnail/{video.Id}";
            
            var audio = new Audio()
            {
                Id = audioId,
                Url = $"/api/audios/{audioId}",
                SourceId = video.Id,
                MsDuration = (long)audioInfo.Duration.TotalMilliseconds
            };
            await _audioRepository.CreateAsync(audio);
            await _audioRepository.SaveChangesAsync();
        }

        await _videoRepository.UpdateAsync(video);
        await _videoRepository.SaveChangesAsync();

    }
}