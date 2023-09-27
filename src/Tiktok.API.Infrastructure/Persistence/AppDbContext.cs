using System.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tiktok.API.Domain.Common.Constants;
using Tiktok.API.Domain.Entities;
using Tiktok.API.Domain.Entities.Interfaces;
using Tiktok.API.Infrastructure.Entities.ModalBuilders;

namespace Tiktok.API.Infrastructure.Persistence;
#nullable disable
public class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public IDbConnection Connection => Database.GetDbConnection();
    
    public DbSet<Video> Videos { get; set; }
    public DbSet<Audio> Audios { get; set; } 
    public DbSet<UserFollower> UserFollowers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        builder.ApplyIdentityConfiguration();
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();
            if (tableName != null && tableName.StartsWith("AspNet"))
            {
                entityType.SetTableName($"{SystemConstants.IdentitySchema}.{tableName[6..]}");
            }
        }
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var modifiedEntries = ChangeTracker.Entries()
            .Where(x => x is { Entity: IDateTracking, State: EntityState.Added or EntityState.Modified });

        foreach (var entry in modifiedEntries)
        {
            var entity = entry.Entity as IDateTracking;
            switch (entry.State)
            {
                case EntityState.Added:
                    entity!.CreatedDate = DateTimeOffset.UtcNow;
                    break;
                case EntityState.Modified:
                    entity!.ModifiedDate = DateTimeOffset.UtcNow;
                    break;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}