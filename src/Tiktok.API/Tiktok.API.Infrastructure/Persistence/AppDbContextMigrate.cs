using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Tiktok.API.Domain.Entities;
using Tiktok.API.Infrastructure.Persistence.Seeds;

namespace Tiktok.API.Infrastructure.Persistence;

public static class AppDbContextMigrate
{
    public static IHost MigrateDatabase(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger>();

        try
        {
            logger.Information("Migrating database associated with context {DbContextName}", nameof(AppDbContext));
            if (context.Database.IsSqlServer())
            {
                context.Database.Migrate();
                RoleSeed.SeedAsync(context, roleManager).Wait();
                UserSeed.SeedAsync(context, userManager).Wait();
            }
            else
            {
                throw new Exception("Database Provider not supported");
            }
        }
        catch (Exception e)
        {
            logger.Error("An Error occured while migration {DbContextName}", nameof(AppDbContext));
            throw;
        }

        return host;
    }
}