using System.Linq.Expressions;
using Tiktok.API.Domain.Entities;

namespace Tiktok.API.Application.Common.Repositories;

public interface IUserRepository
{
    Task<User> CreateUserAsync(User user, string password);
    Task<User> GetUserByIdAsync(string id);
    Task<User> GetUserByEmailAsync(string email);
    Task<User> GetUserByUserNameAsync(string userName);
    Task<User> LoginAsync(string email, string password);
    Task ChangePasswordAsync(string userId, string oldPassword, string newPassword);
    Task ChangePasswordAsync(string email, string newPassword);
    Task<bool> CheckUserExistAsync(Expression<Func<User, bool>> expression);
    
    Task<bool> FollowUserAsync(User followerUser, User followingUser);
    
    Task<bool> UnfollowUserAsync(User followerUser, User followingUser);
    
    Task<bool> CheckFollowUserAsync(User followerUser, User followingUser);
    
    Task<IEnumerable<User>> GetFollowingWithPagingAsync(string followerId, int pageIndex, int pageSize, string? queryString = null);
    
    Task<IEnumerable<User>> GetFollowersWithPagingAsync(string followingId, int pageIndex, int pageSize);

}