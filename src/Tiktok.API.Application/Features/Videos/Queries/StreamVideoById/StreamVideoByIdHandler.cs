using MediatR;
using Microsoft.AspNetCore.Http;
using Tiktok.API.Application.Common.Repositories;
using Tiktok.API.Domain.Configurations;
using Tiktok.API.Domain.Exceptions;
using Tiktok.API.Domain.Services;

namespace Tiktok.API.Application.Features.Videos.Queries.StreamVideoById;

public class StreamVideoByIdHandler : IRequestHandler<StreamVideoByIdQuery, string>
{
    private readonly IFileService _fileService;
    private readonly IVideoRepository _videoRepository;

    public StreamVideoByIdHandler(IFileService fileService, IVideoRepository videoRepository)
    {
        _fileService = fileService;
        _videoRepository = videoRepository;
    }

    public async Task<string> Handle(StreamVideoByIdQuery request, CancellationToken cancellationToken)
    {
        var videoInDatabase = _videoRepository.VideoExistsAsync(request.Id);
        var fileName = $"{request.Id}.mp4";
        var videoInStorage = _fileService.CheckFileExistAsync(fileName, DiskStorageSettings.Video);
        var task = await Task.WhenAll(videoInDatabase, videoInStorage);
        if(!task[0] && !task[1])
            throw new HttpException("Video not exist", StatusCodes.Status404NotFound);
        
        var result =  await _fileService.GetFilePathAsync(fileName, DiskStorageSettings.Video);
        return result;
    }
}