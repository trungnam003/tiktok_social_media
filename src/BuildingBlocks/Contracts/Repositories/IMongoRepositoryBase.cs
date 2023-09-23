using System.Linq.Expressions;
using Contracts.Domains.Interfaces;

namespace Contracts.Repositories;

public interface IMongoRepositoryBase<T>  where T : IMongoEntityBase
{
    Task AddAsync(T entity);
    Task<T> FindOneAsync(Expression<Func<T, bool>> predicate);
    Task UpdateAsync(T entity);
    Task DeleteAsync(string id);
}