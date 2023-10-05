namespace Tiktok.API.Domain.Models;

public class UserFollowerCache
{
    public static readonly string Schema = "user_follower_cache";
    public long FollowerCount { get; set; }
    public long FollowingCount { get; set; }
}