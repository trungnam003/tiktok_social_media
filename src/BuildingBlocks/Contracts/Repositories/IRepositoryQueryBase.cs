
using System.Linq.Expressions;
using Contracts.Domains.Interfaces;

namespace Tiktok.API.Domain.Repositories;

public interface IRepositoryQueryBase<T, in TK>
    where T : IEntityBase<TK>
{
    IQueryable<T> FindAll(bool trackChanges = false);
    IQueryable<T> FindAll(bool trackChanges = false, params Expression<Func<T, object[]>>[] includes);
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false);

    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false,
        params Expression<Func<T, object[]>>[] includes);

    Task<T?> GetByIdAsync(TK id, bool trackChanges = false);
    Task<T?> GetByIdAsync(TK id, bool trackChanges = false, params Expression<Func<T, object[]>>[] includes);
}
