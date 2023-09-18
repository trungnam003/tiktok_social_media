using Contracts.Domains.Interfaces;
using Contracts.Repositories;
using Microsoft.EntityFrameworkCore;


using Tiktok.API.Domain.Repositories;

namespace Tiktok.API.Infrastructure.Repositories.Abstracts;

/// <summary>
///     RepositoryQueryCommand
///     No SaveChanges() automatically
/// </summary>
/// <typeparam name="T">Entity Base</typeparam>
/// <typeparam name="TK">Type of Id</typeparam>
/// <typeparam name="TContext">DbContext</typeparam>
public abstract class RepositoryQueryCommand<T, TK, TContext> : RepositoryQuery<T, TK, TContext>,
    IRepositoryCommand<T, TK, TContext>
    where T : class, IEntityBase<TK>
    where TContext : DbContext
{
    private readonly DbContext _context;
    private readonly DbSet<T> _dbSet;
    public IUnitOfWork<TContext> UnitOfWork { get; }

    protected RepositoryQueryCommand(DbContext context, IUnitOfWork<TContext> unitOfWork) : base(context)
    {
        _context = context;
        UnitOfWork = unitOfWork;
        _dbSet = context.Set<T>();
    }

    #region Sync Methods


    public virtual TK Create(T entity)
    {
        var result = _dbSet.Add(entity);
        return result.Entity.Id;
    }

    public virtual IList<TK> CreateList(IEnumerable<T> entities)
    {
        _dbSet.AddRange(entities);
        var result = entities.Select(x => x.Id).ToList();
        return result;
    }

    public virtual bool Update(T entity)
    {
        if (_context.Entry(entity).State == EntityState.Unchanged)
            return false;
        var entityExist = _dbSet.Find(entity.Id);
        if (entityExist == null)
            return false;
        _context.Entry(entityExist).CurrentValues.SetValues(entity);
        return true;
    }

    public virtual bool Delete(T entity)
    {
        var entityExist = _dbSet.Find(entity.Id);
        if (entityExist == null)
            return false;
        _dbSet.Remove(entityExist);
        return true;
    }

    public virtual bool DeleteById(TK id)
    {
        var entityExist = _dbSet.Find(id);
        if (entityExist == null)
            return false;
        _dbSet.Remove(entityExist);
        return true;
    }

    public virtual bool DeleteList(IEnumerable<T> entities)
    {
        _dbSet.RemoveRange(entities);
        return true;
    }

    public virtual int SaveChanges()
    {
        return _context.SaveChanges();
    }

    #endregion


    #region Async Methods

    public virtual async Task<TK> CreateAsync(T entity)
    {
        var result = await _dbSet.AddAsync(entity);
        return result.Entity.Id;
    }

    public virtual async Task<IList<TK>> CreateListAsync(IEnumerable<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
        return entities.Select(e => e.Id).ToList();
    }

    public virtual async Task<bool> UpdateAsync(T entity)
    {
        if (_context.Entry(entity).State == EntityState.Unchanged)
            return false;

        var entityExist = await _dbSet.FindAsync(entity.Id);
        if (entityExist == null)
            return false;

        _context.Entry(entityExist).CurrentValues.SetValues(entity);

        return true;
    }

    public virtual async Task<bool> DeleteAsync(T entity)
    {
        var entityExist = await _dbSet.FindAsync(entity.Id);
        if (entityExist == null)
            return false;

        _dbSet.Remove(entityExist);
        return true;
    }

    public virtual async Task<bool> DeleteByIdAsync(TK id)
    {
        var entityExist = await _dbSet.FindAsync(id);
        if (entityExist == null)
            return false;

        _dbSet.Remove(entityExist);
        return true;
    }

    public virtual Task<bool> DeleteListAsync(IEnumerable<T> entities)
    {
        _context.Set<T>().RemoveRange(entities);
        return Task.FromResult(true);
    }

    public virtual Task<int> SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }

    #endregion

}