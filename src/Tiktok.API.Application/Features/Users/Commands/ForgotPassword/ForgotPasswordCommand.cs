using MediatR;
using Tiktok.API.Domain.Common.Models;

namespace Tiktok.API.Application.Features.Users.Commands.ForgotPassword;
#nullable disable
public class ForgotPasswordCommand : IRequest<ApiSuccessResult<string>>
{
    public string Email { get; set; }
}