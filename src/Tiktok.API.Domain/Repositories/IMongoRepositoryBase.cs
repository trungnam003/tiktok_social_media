using System.Linq.Expressions;
using Tiktok.API.Domain.Entities.Interfaces;

namespace Tiktok.API.Domain.Repositories;

public interface IMongoRepositoryBase<T>  where T : IMongoEntityBase
{
    Task AddAsync(T entity);
    Task<T> FindOneAsync(Expression<Func<T, bool>> predicate);
    Task UpdateAsync(T entity);
    Task DeleteAsync(string id);
}