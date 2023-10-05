using AutoMapper;
using Tiktok.API.Application.Common.Mappers;
using Tiktok.API.Domain.Entities;

namespace Tiktok.API.Application.Common.DTOs.Users;
#nullable disable
public class UserDto : IMapFrom<User>
{
    public string UserName { get; set; }
    public string FullName { get; set; }
    public string ImageUrl { get; set; }
    public string Id { get; set; }
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, UserDto>().ReverseMap();
    }
}
