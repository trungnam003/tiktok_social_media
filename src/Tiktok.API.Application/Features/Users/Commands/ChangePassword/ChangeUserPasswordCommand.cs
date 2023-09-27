using MediatR;
using Tiktok.API.Domain.Common.Models;

namespace Tiktok.API.Application.Features.Users.Commands.ChangePassword;
#nullable disable
public class ChangeUserPasswordCommand : IRequest<ApiSuccessResult<string>>
{
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmNewPassword { get; set; }
    private string _userId;
    
    public void SetUserId(string userId)
    {
        _userId = userId;
    }
    
    public string GetUserId()
    {
        return _userId;
    }
    
}