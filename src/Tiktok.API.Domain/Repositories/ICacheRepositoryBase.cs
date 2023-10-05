namespace Tiktok.API.Domain.Repositories;

public interface ICacheRepositoryBase<T> where T : class
{
    Task<T> GetAsync(string key);
    Task SetAsync(string key, T value, TimeSpan? expiry = null);
    Task RemoveAsync(string key);
    Task<bool> ExistsAsync(string key);
    Task ClearAsync();
}