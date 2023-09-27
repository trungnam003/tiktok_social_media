using System.ComponentModel.DataAnnotations.Schema;
using Tiktok.API.Domain.Common.Constants;

namespace Tiktok.API.Domain.Entities;
#nullable disable
[Table("UserFollowers",Schema= SystemConstants.AppSchema)]
public class UserFollower
{
    public string FollowerId { get; set; }
    public User Follower { get; set; }
    public string FollowingId { get; set; }
    public User Following { get; set; }
}