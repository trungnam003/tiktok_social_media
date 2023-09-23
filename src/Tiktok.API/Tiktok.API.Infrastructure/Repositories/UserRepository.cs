using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Tiktok.API.Application.Common.Repositories;
using Tiktok.API.Domain.Common.Constants;
using Tiktok.API.Domain.Entities;
using Tiktok.API.Domain.Exceptions;
using Tiktok.API.Infrastructure.Persistence;

namespace Tiktok.API.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;
    private readonly AppDbContext _context;

    public UserRepository(UserManager<User> userManager, SignInManager<User> signInManager, AppDbContext context)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        _context = context;
    }

    public async Task<User> CreateUserAsync(User user, string password)
    {
        var userExist = await _userManager.Users
            .Where(u => u.UserName == user.UserName || u.Email == user.Email)
            .FirstOrDefaultAsync();
        if (userExist != null) throw new HttpException("Email already exists", StatusCodes.Status400BadRequest);

        if (user.FullName.IsNullOrEmpty()) user.FullName = user.UserName;

        await _userManager.CreateAsync(user, password);
        await _userManager.AddToRoleAsync(user, "User");
        await _userManager.AddClaimsAsync(user, new Claim[]
        {
            new(SystemConstants.AppClaims.FullName, user.FullName),
            new(SystemConstants.AppClaims.UserName, user.UserName),
            new(SystemConstants.AppClaims.Email, user.Email),
            new(SystemConstants.AppClaims.Id, user.Id),
            new(SystemConstants.AppClaims.Roles, "User")
        });
        return user;
    }

    public async Task<User> GetUserByIdAsync(string id)
    {
        return await _userManager.FindByIdAsync(id);
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        var result = await _userManager.FindByEmailAsync(email);
        return result;
    }

    public Task<User> GetUserByUserNameAsync(string userName)
    {
        throw new NotImplementedException();
    }

    public async Task<User> LoginAsync(string email, string password)
    {
        var user = await GetUserByEmailAsync(email);
        if (user == null) throw new HttpException("User not found", StatusCodes.Status404NotFound);
        var checkedPassword = await _signInManager.CheckPasswordSignInAsync(user, password, false);
        if (checkedPassword.Succeeded) return user;
        throw new HttpException("Password is incorrect", StatusCodes.Status401Unauthorized);
    }
}