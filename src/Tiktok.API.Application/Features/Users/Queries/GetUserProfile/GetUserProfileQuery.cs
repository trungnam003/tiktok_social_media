using MediatR;
using Tiktok.API.Application.Common.DTOs.Users;
using Tiktok.API.Domain.Common.Models;

namespace Tiktok.API.Application.Features.Users.Queries.GetUserProfile;
#nullable disable
public class GetUserProfileQuery : IRequest<ApiSuccessResult<ProfileUserDto>>
{
    public string UserName { get; set; }
    
    private bool _isSelf = false;
    private string _userId = string.Empty;
    
    public void SetUserId(string userId)
    {
        this._userId = userId;
    }
    
    public string GetUserId()
    {
        return this._userId;
    }
    
    public void SetSelf(bool isSelf)
    {
        this._isSelf = isSelf;
    }
    
    public bool IsSelf()
    {
        return this._isSelf;
    }
}