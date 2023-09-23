using Tiktok.API.Domain.Entities;

namespace Tiktok.API.Application.Common.Repositories;

public interface IUserRepository
{
    Task<User> CreateUserAsync(User user, string password);
    Task<User> GetUserByIdAsync(string id);
    Task<User> GetUserByEmailAsync(string email);
    Task<User> GetUserByUserNameAsync(string userName);

    Task<User> LoginAsync(string email, string password);
}