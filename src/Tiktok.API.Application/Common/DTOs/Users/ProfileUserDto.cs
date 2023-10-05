using AutoMapper;
using Tiktok.API.Application.Common.Mappers;
using Tiktok.API.Domain.Entities;

namespace Tiktok.API.Application.Common.DTOs.Users;

public class ProfileUserDto : IMapFrom<User>
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string FullName { get; set; }
    public string ImageUrl { get; set; }
    public string Bio { get; set; }
    public long FollowerCount { get; set; }
    public long FollowingCount { get; set; }
    public bool Self { get; set; } = false;
    public bool Following { get; set; }= false;
    public bool Followed { get; set; }= false;
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, ProfileUserDto>().ReverseMap();
    }
}