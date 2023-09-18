using Contracts.Domains.Interfaces;

namespace Tiktok.API.Domain.Repositories;

public interface IMongoRepository<T> where T : IEntityBase<T>
{
    Task<T> GetAsync(string id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<bool> AddAsync(T entity);
    Task<bool> UpdateAsync(T entity);
    Task<bool> DeleteAsync(T entity);
}