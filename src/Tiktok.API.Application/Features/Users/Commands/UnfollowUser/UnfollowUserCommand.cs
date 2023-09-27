using MediatR;
using Tiktok.API.Domain.Common.Models;

namespace Tiktok.API.Application.Features.Users.Commands.UnfollowUser;
#nullable disable
public class UnfollowUserCommand : IRequest<ApiSuccessResult<string>>
{
    /// <summary>
    /// is source user following target user
    /// </summary>
    public string UnfollowingId { get; set; }

    /// <summary>
    /// is target user being followed by source user
    /// </summary>
    private string _followerId;
    public void SetFollowerId(string follower)
    {
        _followerId = follower;
    }
    
    public string GetFollowerId()
    {
        return _followerId;
    }
}