using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tiktok.API.Domain.Common.Constants;
using Tiktok.API.Domain.Entities;

namespace Tiktok.API.Infrastructure.Entities.Configurations;

public class UserFollowConfiguration : IEntityTypeConfiguration<UserFollower>
{
    public void Configure(EntityTypeBuilder<UserFollower> builder)
    {

        builder.HasKey(x => new { x.FollowerId, x.FollowingId });

        builder.HasOne(x => x.Follower)
            .WithMany(x => x.Followers)
            .HasForeignKey(x => x.FollowerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Following)
            .WithMany(x => x.Followings)
            .HasForeignKey(x => x.FollowingId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}