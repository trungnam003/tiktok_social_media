using Tiktok.API.Domain.Entities.Abstracts;

namespace Tiktok.API.Domain.Common.Repositories;

public interface IMongoRepository<T> where T : MongoEntity
{
    Task<T> GetAsync(string id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<bool> AddAsync(T entity);
    Task<bool> UpdateAsync(T entity);
    Task<bool> DeleteAsync(T entity);
}