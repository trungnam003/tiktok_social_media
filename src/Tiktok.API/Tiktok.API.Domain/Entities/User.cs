#nullable disable

using Contracts.Domains.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Tiktok.API.Domain.Entities;

public class User : IdentityUser, IEntityBase<string>
{
    public string FullName { get; set; }
    public string Story { get; set; }
    public string ImageUrl { get; set; }
    public long FollowerCount { get; set; }
    public long FollowingCount { get; set; }
}