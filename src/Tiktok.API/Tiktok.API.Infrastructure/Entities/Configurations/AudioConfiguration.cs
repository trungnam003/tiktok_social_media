using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tiktok.API.Domain.Common.Constants;
using Tiktok.API.Domain.Entities;

namespace Tiktok.API.Infrastructure.Entities.Configurations;

public class AudioConfiguration : IEntityTypeConfiguration<Audio>
{
    public void Configure(EntityTypeBuilder<Audio> builder)
    {
        builder.ToTable("Audios", SystemConstants.AppSchema)
            .HasKey(x => x.Id);

        // create relationship one one
        builder.HasOne(x => x.Source)
            .WithOne(x => x.Audio)
            .HasForeignKey<Audio>(x => x.SourceId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}