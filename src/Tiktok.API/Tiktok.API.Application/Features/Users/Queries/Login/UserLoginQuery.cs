using MediatR;
using Tiktok.API.Application.Common.DTOs.Users;
using Tiktok.API.Domain.Common.Models;

#nullable disable
namespace Tiktok.API.Application.Features.Users.Queries.Login;

public class UserLoginQuery : IRequest<ApiSuccessResult<LoginResultDto>>
{
    public string Email { get; set; }
    public string Password { get; set; }
}