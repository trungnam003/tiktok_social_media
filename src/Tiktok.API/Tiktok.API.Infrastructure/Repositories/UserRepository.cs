using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Tiktok.API.Application.Common.Repositories;
using Tiktok.API.Domain.Entities;
using Tiktok.API.Domain.Exceptions;

namespace Tiktok.API.Infrastructure.Repositories;

public class UserRepository :  IUserRepository
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public UserRepository(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
    }

    public async Task<User> CreateUserAsync(User user, string password)
    {
        await _userManager.CreateAsync(user, password);
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