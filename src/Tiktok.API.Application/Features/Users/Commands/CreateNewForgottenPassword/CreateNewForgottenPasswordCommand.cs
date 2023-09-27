using MediatR;
using Tiktok.API.Domain.Common.Models;

namespace Tiktok.API.Application.Features.Users.Commands.CreateNewForgottenPassword;
#nullable disable
public class CreateNewForgottenPasswordCommand : IRequest<ApiSuccessResult<string>>
{
    public string Otp { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmNewPassword { get; set; }
    
    public string Email { get; set; }
}