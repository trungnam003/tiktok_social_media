using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Tiktok.API.Application.Common.Repositories;
using Tiktok.API.Domain.Common.Models;
using Tiktok.API.Domain.Entities;
using Tiktok.API.Domain.EventBusMessages.Events;
using Tiktok.API.Domain.Exceptions;
using Tiktok.API.Domain.Repositories;
using Tiktok.API.Domain.Services;

namespace Tiktok.API.Application.Features.Videos.Commands.UploadVideo;

public class UploadVideoHandler : IRequestHandler<UploadVideoCommand, ApiSuccessResult<string>>
{
    private readonly IFileService _fileService;
    private readonly IUserRepository _userRepository;
    private readonly IVideoRepository _videoRepository;
    private readonly IPublishMessageService _publishMessageService;
    private readonly IAudioRepository _audioRepository;
    private readonly ISerializeService _serializeService;
    private readonly IVideoTagRepository _videoTagRepository;


    public UploadVideoHandler(IFileService fileService, IUserRepository userRepository,
        IVideoRepository videoRepository, IPublishMessageService publishMessageService, IAudioRepository audioRepository, ISerializeService serializeService, IVideoTagRepository videoTagRepository)
    {
        _fileService = fileService;
        _userRepository = userRepository;
        _videoRepository = videoRepository;
        _publishMessageService = publishMessageService;
        _audioRepository = audioRepository;
        _serializeService = serializeService;
        _videoTagRepository = videoTagRepository;
        
    }

    public async Task<ApiSuccessResult<string>> Handle(UploadVideoCommand request, CancellationToken cancellationToken)
    {
        var user = await  _userRepository.GetUserByIdAsync(request.GetOwner());
        
        if (request.UseExternalAudio)
        {
            var audio = await _audioRepository.FindByCondition(x => x.Id == request.ExternalAudioId).FirstOrDefaultAsync(cancellationToken: cancellationToken);
            if (audio == null) throw new HttpException("Audio not found", StatusCodes.Status404NotFound);
        }
        if (user == null) throw new HttpException("User not found", StatusCodes.Status404NotFound);
        
        var id = Guid.NewGuid().ToString();
        
        var fileName = request.UseExternalAudio ? $"{id}_temp.mp4" : $"{id}.mp4";
        
        var url = $"/api/videos/{id}";

        var tags = string.Empty;

        if (request.Tags is { Count: > 0 })
        {
            await _videoTagRepository.BulkCreateVideoTagIfNotExist(request.Tags);
            tags = _serializeService.Serialize(request.Tags);
        }
        
        var video = new Video()
        {
            Id = id,
            Title = request.Title,
            OwnerId = user.Id,
            Url = url,
            Tags = tags
        };
        await _fileService.SaveVideoToStorageAsync(request.VideoUpload, fileName);
        await _videoRepository.CreateVideoAsync(video);
        var message = new UserUploadVideoEvent()
        {
            VideoId = id,
            VideoName = fileName,
            UseExternalAudio = request.UseExternalAudio,
            ExternalAudioId = request.UseExternalAudio ? request.ExternalAudioId : null
        };
        await _publishMessageService.Publish(message, cancellationToken);
        return new ApiSuccessResult<string>(url);
    }
}