using Microsoft.AspNetCore.Identity;

namespace Tiktok.API.Infrastructure.Persistence.Seeds;

public static class RoleSeed
{
    public static async Task SeedAsync(AppDbContext context, RoleManager<IdentityRole> roleManager)
    {
        if (!context.Roles.Any())
            foreach (var role in GetRoles())
                await roleManager.CreateAsync(role);
    }

    private static IEnumerable<IdentityRole> GetRoles()
    {
        var roles = new List<IdentityRole>
        {
            new("Admin"),
            new("User")
        };
        return roles;
    }
}