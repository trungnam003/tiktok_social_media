using AutoMapper;
using MediatR;
using Tiktok.API.Application.Common.DTOs.Videos;
using Tiktok.API.Application.Common.Repositories;
using Tiktok.API.Domain.Common.Models;

namespace Tiktok.API.Application.Features.Videos.Queries.GetVideoById;

public class GetVideoByIdHandler : IRequestHandler<GetVideoByIdQuery, ApiSuccessResult<VideoDto>>
{
    private readonly IVideoRepository _videoRepository;
    private readonly IMapper _mapper;
    public GetVideoByIdHandler(IVideoRepository videoRepository, IMapper mapper)
    {
        _videoRepository = videoRepository;
        _mapper = mapper;
    }

    public async Task<ApiSuccessResult<VideoDto>> Handle(GetVideoByIdQuery request, CancellationToken cancellationToken)
    {
        var video = await _videoRepository.GetByIdAsync(request.Id);
        var videoDto = _mapper.Map<VideoDto>(video);
        return new ApiSuccessResult<VideoDto>(videoDto);
    }
}