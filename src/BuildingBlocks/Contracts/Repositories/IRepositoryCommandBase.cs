using Contracts.Domains.Interfaces;


namespace Contracts.Repositories;

public interface IRepositoryCommandBase<in T, TK>
    where T : IEntityBase<TK>
{
    #region Sync Methods

    TK Create(T entity);
    IList<TK> CreateList(IEnumerable<T> entities);
    bool Update(T entity);
    bool Delete(T entity);
    bool DeleteById(TK id);
    bool DeleteList(IEnumerable<T> entities);
    int SaveChanges();

    #endregion

    #region Async Methods

    Task<TK> CreateAsync(T entity);
    Task<IList<TK>> CreateListAsync(IEnumerable<T> entities);
    Task<bool> UpdateAsync(T entity);
    Task<bool> DeleteAsync(T entity);
    Task<bool> DeleteByIdAsync(TK id);
    Task<bool> DeleteListAsync(IEnumerable<T> entities);
    Task<int> SaveChangesAsync();

    #endregion
}
