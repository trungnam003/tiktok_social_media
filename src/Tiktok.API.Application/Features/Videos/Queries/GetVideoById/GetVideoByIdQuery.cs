using MediatR;
using Tiktok.API.Application.Common.DTOs.Videos;
using Tiktok.API.Domain.Common.Models;

namespace Tiktok.API.Application.Features.Videos.Queries.GetVideoById;
#nullable disable
public class GetVideoByIdQuery : IRequest<ApiSuccessResult<VideoDto>>
{
    public string Id { get; set; }
}