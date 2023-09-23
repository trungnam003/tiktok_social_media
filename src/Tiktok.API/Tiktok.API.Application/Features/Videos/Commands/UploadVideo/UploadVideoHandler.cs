using Contracts.EventBusMessages.Events;
using MediatR;
using Microsoft.AspNetCore.Http;
using Tiktok.API.Application.Common.Repositories;
using Tiktok.API.Domain.Common.Models;
using Tiktok.API.Domain.Entities;
using Tiktok.API.Domain.Exceptions;
using Tiktok.API.Domain.Services;

namespace Tiktok.API.Application.Features.Videos.Commands.UploadVideo;

public class UploadVideoHandler : IRequestHandler<UploadVideoCommand, ApiSuccessResult<string>>
{
    private readonly IFileService _fileService;
    private readonly IUserRepository _userRepository;
    private readonly IVideoRepository _videoRepository;
    private readonly IPublishMessageService _publishMessageService;


    public UploadVideoHandler(IFileService fileService, IUserRepository userRepository,
        IVideoRepository videoRepository, IPublishMessageService publishMessageService)
    {
        _fileService = fileService;
        _userRepository = userRepository;
        _videoRepository = videoRepository;
        _publishMessageService = publishMessageService;
    }

    public async Task<ApiSuccessResult<string>> Handle(UploadVideoCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByIdAsync(request.GetOwner());
        if (user == null) throw new HttpException("User not found", StatusCodes.Status404NotFound);
        
        var id = Guid.NewGuid().ToString();
        
        var fileName = request.UseExternalAudio ? $"{id}_temp.mp4" : $"{id}.mp4";
        
        await _fileService.SaveVideoToStorageAsync(request.VideoUpload, fileName);
        var url = $"/api/videos/{id}";
        var video = new Video()
        {
            Id = id,
            Title = request.Title,
            OwnerId = user.Id,
            Url = url,
            TotalLove = 0,
        };
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