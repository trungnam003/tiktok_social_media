using AutoMapper;
using MediatR;
using Tiktok.API.Application.Common.DTOs.Users;
using Tiktok.API.Application.Common.Mappers;
using Tiktok.API.Domain.Common.Models;
using Tiktok.API.Domain.Entities;

namespace Tiktok.API.Application.Features.Users.Commands.Register;
#nullable disable
public class RegisterUserCommand : IRequest<ApiSuccessResult<RegisterResultDto>>, IMapFrom<User>
{
    public string FullName { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<RegisterUserCommand, User>().ReverseMap();
    }
}