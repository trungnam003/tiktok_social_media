using AutoMapper;
using Tiktok.API.Application.Common.Mappers;
using Tiktok.API.Domain.Entities;

namespace Tiktok.API.Application.Common.DTOs.Users;
#nullable disable
public class RegisterResultDto : IMapFrom<User>
{
    public string Username { get; set; }
    public string Email { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<RegisterResultDto, User>().ReverseMap();
    }
}