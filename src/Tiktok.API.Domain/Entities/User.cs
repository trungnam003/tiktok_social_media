#nullable disable

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Tiktok.API.Domain.Common.Constants;
using Tiktok.API.Domain.Entities.Interfaces;

namespace Tiktok.API.Domain.Entities;

public class User : IdentityUser, IEntityBase<string>
{
    public string FullName { get; set; }
    public string Bio { get; set; }
    public string ImageUrl { get; set; }
    [NotMapped] public virtual ICollection<Video> Videos { get; set; }
    [NotMapped] public virtual ICollection<Audio> Audios { get; set; }

    
    [NotMapped]
    public virtual ICollection<UserFollower> Followers { get; set; }
    
    [NotMapped]
    public virtual ICollection<UserFollower> Followings { get; set; }
}