using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Tiktok.API.Domain.Common.Constants;
using Tiktok.API.Domain.Entities;

namespace Tiktok.API.Infrastructure.Persistence.Seeds;

public static class UserSeed
{
    public static async Task SeedAsync(AppDbContext context, UserManager<User> userManager)
    {
        if (!context.Users.Any())
        {
            foreach (var user in GetUsers())
            {
                await userManager.CreateAsync(user, "1234");
                await userManager.AddToRoleAsync(user, "User");
                await userManager.AddClaimsAsync(user, new Claim[]
                {
                    new(SystemConstants.AppClaims.FullName, user.FullName),
                    new(SystemConstants.AppClaims.UserName, user.UserName),
                    new(SystemConstants.AppClaims.Email, user.Email),
                    new(SystemConstants.AppClaims.Id, user.Id),
                    new(SystemConstants.AppClaims.Roles, "User")
                });
            }

            foreach (var user in GetAdmins())
            {
                await userManager.CreateAsync(user, "1234");
                await userManager.AddToRoleAsync(user, "Admin");
                await userManager.AddClaimsAsync(user, new Claim[]
                {
                    new(SystemConstants.AppClaims.FullName, user.FullName),
                    new(SystemConstants.AppClaims.UserName, user.UserName),
                    new(SystemConstants.AppClaims.Email, user.Email),
                    new(SystemConstants.AppClaims.Id, user.Id),
                    new(SystemConstants.AppClaims.Roles, "Admin")
                });
            }
        }
    }


    private static IEnumerable<User> GetUsers()
    {
        var users = new List<User>
        {
            new()
            {
                Email = "thtn.1611.dev@gmail.com",
                FullName = "Trung Nam",
                UserName = "trungnam",
                PhoneNumber = "0865346424",
                EmailConfirmed = true
            },
            new()
            {
                Email = "yeevuko@mailto.plus",
                FullName = "Tokuda Shusei",
                UserName = "tokuda123",
                PhoneNumber = "0865924423",
                EmailConfirmed = false
            }
        };
        return users;
    }

    private static IEnumerable<User> GetAdmins()
    {
        var admins = new List<User>
        {
            new()
            {
                Email = "thtntrungnam@gmail.com",
                FullName = "Trung Nam",
                UserName = "admin",
                PhoneNumber = "0865346422",
                EmailConfirmed = true
            }
        };
        return admins;
    }
}