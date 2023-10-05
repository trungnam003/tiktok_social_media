using MediatR;
using Microsoft.AspNetCore.Http;
using Tiktok.API.Application.Common.Repositories;
using Tiktok.API.Domain.Configurations;
using Tiktok.API.Domain.Exceptions;
using Tiktok.API.Domain.Services;

namespace Tiktok.API.Application.Features.Videos.Commands.DeleteVideo;

public class DeleteVideoHandler : IRequestHandler<DeleteVideoCommand, bool>
{
    private readonly IVideoRepository _videoRepository;
    private readonly IFileService _fileService;

    public DeleteVideoHandler(IVideoRepository videoRepository, IFileService fileService)
    {
        _videoRepository = videoRepository;
        _fileService = fileService;
    }


    public async Task<bool> Handle(DeleteVideoCommand request, CancellationToken cancellationToken)
    {
        var video = await _videoRepository.GetByIdAsync(request.Id);
        if (video == null)
        {
            throw new HttpException("Video not found", StatusCodes.Status404NotFound);
        }
        if(video.OwnerId != request.GetOwner())
        {
            throw new ForbiddenException("You are not the owner of this video");
        }

        var videoFileName = $"{video.Id}.mp4";
        var thumbnailFileName = $"{video.Id}.png";

        await Task.WhenAll(_fileService.DeleteFileAsync(videoFileName, DiskStorageSettings.Video),
            _fileService.DeleteFileAsync(thumbnailFileName, DiskStorageSettings.Thumbnail));
        
        return await _videoRepository.DeleteAsync(video);
    }
}