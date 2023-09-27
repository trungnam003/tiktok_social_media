using AutoMapper;
using Tiktok.API.Application.Common.Mappers;
using Tiktok.API.Domain.Entities;

namespace Tiktok.API.Application.Common.DTOs.Videos;
#nullable disable
public class VideoDto : IMapFrom<Video>
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Tags { get; set; }
    public string StreamUrl { get; set; }
    public string ThumbnailUrl { get; set; }
    public long MsDuration{ get; set; }
    public long TotalLove { get; set; }
    public string? ExternalAudioId { get; set; }
    public string OwnerId { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Video, VideoDto>()
            .ForMember(d => d.StreamUrl, opt => opt.MapFrom(s => $"{s.Url}/stream"));
    }
}