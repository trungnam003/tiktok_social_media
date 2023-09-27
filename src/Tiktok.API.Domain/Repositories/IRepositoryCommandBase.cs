using Microsoft.EntityFrameworkCore;
using Tiktok.API.Domain.Entities.Interfaces;

namespace Tiktok.API.Domain.Repositories;

public interface IRepositoryCommandBase<T, TK>
    where T : IEntityBase<TK>
{
    #region Sync Methods

    TK Create(T entity);
    IList<TK> CreateList(IEnumerable<T> entities);
    bool Update(T entity);
    bool Delete(T entity);
    bool DeleteList(IEnumerable<T> entities);
    int SaveChanges();

    #endregion

    #region Async Methods

    Task<TK> CreateAsync(T entity);
    Task<IList<TK>> CreateListAsync(IEnumerable<T> entities);
    Task<bool> UpdateAsync(T entity);
    Task<bool> DeleteAsync(T entity);
    Task<bool> DeleteListAsync(IEnumerable<T> entities);
    Task<int> SaveChangesAsync();

    #endregion
}

public interface IRepositoryCommand<T, TK, TContext> : IRepositoryCommandBase<T, TK>
    where T : IEntityBase<TK>
    where TContext : DbContext
{
    IUnitOfWork<TContext> UnitOfWork { get; }
}
