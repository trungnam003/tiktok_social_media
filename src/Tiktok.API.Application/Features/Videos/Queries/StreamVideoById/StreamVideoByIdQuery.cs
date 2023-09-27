using MediatR;
using Tiktok.API.Domain.Common.Models;

namespace Tiktok.API.Application.Features.Videos.Queries.StreamVideoById;
#nullable disable
public class StreamVideoByIdQuery : IRequest<string>
{
    public string Id { get; set; }
}