using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Tiktok.API.Domain.Entities.Interfaces;

namespace Tiktok.API.Domain.Common.Repositories;

public interface IRepositoriesCommand<T, TK>
    where T : IEntityBase<TK>
{
    #region Sync Methods

    TK Create(T entity);
    IList<TK> CreateList(IEnumerable<T> entities);
    bool Update(T entity);
    bool Delete(T entity);
    bool DeleteById(TK id);
    bool DeleteList(IEnumerable<T> entities);
    bool DeleteByIdList(IEnumerable<TK> ids);
    int SaveChanges();
    
    #endregion
    
    #region Async Methods
    
    Task<TK> CreateAsync(T entity);
    Task<IList<TK>> CreateListAsync(IEnumerable<T> entities);
    Task<bool> UpdateAsync(T entity);
    Task<bool> DeleteAsync(T entity);
    Task<bool> DeleteByIdAsync(TK id);
    Task<bool> DeleteListAsync(IEnumerable<T> entities);
    Task<bool> DeleteByIdListAsync(IEnumerable<TK> ids);
    Task<int> SaveChangesAsync();
    
    #endregion

    #region Transaction Methods

    Task<IDbContextTransaction> BeginTransactionAsync();
    Task EndTransactionAsync(IDbContextTransaction transaction);
    Task RollbackTransactionAsync(IDbContextTransaction transaction);

    #endregion
}

public interface IRepositoriesCommand<T, TK, TContext>
    where T : IEntityBase<TK>
    where TContext : DbContext
{
    // No Implementation
}