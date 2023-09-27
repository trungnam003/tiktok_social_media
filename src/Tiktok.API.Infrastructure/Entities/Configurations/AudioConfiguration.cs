using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tiktok.API.Domain.Common.Constants;
using Tiktok.API.Domain.Entities;

namespace Tiktok.API.Infrastructure.Entities.Configurations;

public class AudioConfiguration : IEntityTypeConfiguration<Audio>
{
    public void Configure(EntityTypeBuilder<Audio> builder)
    {
        // create relationship one one
        builder.HasOne(x => x.Source)
            .WithOne(x => x.Audio)
            .HasForeignKey<Audio>(x => x.SourceId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne<User>(x => x.Owner)
            .WithMany(x => x.Audios)
            .HasForeignKey(x => x.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}