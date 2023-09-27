using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tiktok.API.Domain.Common.Constants;
using Tiktok.API.Domain.Entities;

namespace Tiktok.API.Infrastructure.Entities.Configurations;

public class VideoConfiguration : IEntityTypeConfiguration<Video>
{
    public void Configure(EntityTypeBuilder<Video> builder)
    {
        // create relationship
        builder.HasOne(x => x.Owner)
            .WithMany(x => x.Videos)
            .HasForeignKey(x => x.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ExternalAudio)
            .WithMany(x => x.UsageVideos)
            .HasForeignKey(x => x.ExternalAudioId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}