using System.Linq.Expressions;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver.Linq;
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
        return _userManager.FindByNameAsync(userName);
    }

    public async Task<User> LoginAsync(string email, string password)
    {
        var user = await GetUserByEmailAsync(email);
        if (user == null) throw new HttpException("User not found", StatusCodes.Status404NotFound);
        var checkedPassword = await _signInManager.CheckPasswordSignInAsync(user, password, false);
        if (checkedPassword.Succeeded) return user;
        throw new HttpException("Password is incorrect", StatusCodes.Status401Unauthorized);
    }

    public async Task ChangePasswordAsync(string userId, string oldPassword, string newPassword)
    {
        var user = await GetUserByIdAsync(userId);
        if (user == null) throw new HttpException("User not found", StatusCodes.Status404NotFound);
        var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        if (!result.Succeeded)
            throw new HttpException("Password is incorrect", StatusCodes.Status401Unauthorized);
    }

    public async Task ChangePasswordAsync(string email, string newPassword)
    {
        var user = await GetUserByEmailAsync(email);
        if (user == null) throw new HttpException("User not found", StatusCodes.Status404NotFound);
        var token = _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token.Result, newPassword);
        if (!result.Succeeded)
            throw new HttpException("An error occurred while creating a new password", StatusCodes.Status500InternalServerError);
    }

    public Task<bool> CheckUserExistAsync(Expression<Func<User, bool>> expression)
    {
        return _userManager.Users.AnyAsync(expression);
    }

    public async Task<bool> FollowUserAsync(User followerUser, User followingUser)
    {
        try
        {
             await _context.UserFollowers.AddAsync(new UserFollower
            {
                FollowerId = followerUser.Id,
                FollowingId = followingUser.Id
            });
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> UnfollowUserAsync(User followerUser, User followingUser)
    {
        try
        {
            var result = _context.UserFollowers.Remove(new UserFollower
            {
                FollowerId = followerUser.Id,
                FollowingId = followingUser.Id
            });
             await _context.SaveChangesAsync();
            return (true);
        }
        catch (Exception e)
        {
            return (false);
        }
    }

    public Task<bool> CheckFollowUserAsync(User followerUser, User followingUser)
    {
        var result = _context.UserFollowers.AnyAsync(x => x.FollowerId == followerUser.Id && x.FollowingId == followingUser.Id);
        return result;
    }

    public async Task<IEnumerable<User>> GetFollowingWithPagingAsync(string followerId, int pageIndex, int pageSize, string? queryString = null)
    {
        var hasFollowing = await _context.UserFollowers.AnyAsync(x => x.FollowerId.Equals(followerId));
        if (!hasFollowing)
            return new List<User>();

        var query = _context.UserFollowers
            .Where(x => x.FollowerId.Equals(followerId))
            .Join(_context.Users,
                follower => follower.FollowingId,
                user => user.Id,
                (follower, users) => users)
            .Select(x => new User()
            {
                UserName = x.UserName,
                FullName = x.FullName,
                ImageUrl = x.ImageUrl,
                Id = x.Id
            });

        if (!string.IsNullOrEmpty(queryString))
        {
            query = query.Where(x =>
                x.UserName.Contains(queryString));
        }

        query = query.Skip((pageIndex - 1) * pageSize)
            .Take(pageSize);
        
        var result = await query.ToListAsync();
        return result;
    }

    public async Task<IEnumerable<User>> GetFollowersWithPagingAsync(string followingId, int pageIndex, int pageSize)
    {
        var hasFollowers = await _context.UserFollowers.AnyAsync(x => x.FollowingId.Equals(followingId));
        if (!hasFollowers)
            return new List<User>();

        var query = _context.UserFollowers
            .Where(x => x.FollowingId.Equals(followingId))
            .Join(_context.Users,
                following => following.FollowerId,
                user => user.Id,
                (following, users) => users)
            .Select(x => new User()
            {
                UserName = x.UserName,
                FullName = x.FullName,
                ImageUrl = x.ImageUrl,
                Id = x.Id
            })
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize);
        
        var result = await query.ToListAsync();
        return result;
    }

    public Task<bool> IsFollowingAsync(string followerId, string followingId)
    {
        return _context.UserFollowers.AnyAsync(x =>
            x.FollowerId.Equals(followerId) && x.FollowingId.Equals(followingId));
    }

    public Task<int> GetFollowingCountAsync(string followerId)
    {
        return _context.UserFollowers.Where(x => x.FollowerId.Equals(followerId)).CountAsync();
    }

    public Task<int> GetFollowersCountAsync(string followingId)
    {
        return _context.UserFollowers.Where(x => x.FollowingId.Equals(followingId)).CountAsync();
    }

    public async Task<IEnumerable<User>> GetUsersInListAsync(IEnumerable<string> userIds,  Expression<Func<User, User>> selector)
    {
        var result = await _context.Users
            .Where(x => userIds.Contains(x.Id))
            .Select(selector)
            .ToListAsync();
        return result;
    }
}