using Tiktok.API.Application.Common.Mappers;
using Tiktok.API.Domain.Entities;

namespace Tiktok.API.Application.Common.DTOs.Users;
#nullable disable
public class LoginResultDto : IMapFrom<User>
{
    public string Token { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string ImageUrl { get; set; }

    public void Mapping(AutoMapper.Profile profile)
    {
        profile.CreateMap<User, LoginResultDto>().ReverseMap();
    }
}