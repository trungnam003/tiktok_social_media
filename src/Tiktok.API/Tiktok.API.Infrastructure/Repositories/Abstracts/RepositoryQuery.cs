using System.Linq.Expressions;
using Contracts.Domains.Interfaces;
using Microsoft.EntityFrameworkCore;
using Tiktok.API.Domain.Repositories;

namespace Tiktok.API.Infrastructure.Repositories.Abstracts;

public abstract class RepositoryQuery<T, TK, TContext> : IRepositoryQuery<T, TK, TContext>
    where T : class, IEntityBase<TK>
    where TContext : DbContext
{
    // private readonly DbContext _context;
    private readonly DbSet<T> _dbSet;

    protected RepositoryQuery(DbContext context)
    {
        // _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual IQueryable<T> FindAll(bool trackChanges = false)
    {
        return trackChanges ? _dbSet : _dbSet.AsNoTracking();
    }

    public virtual IQueryable<T> FindAll(bool trackChanges = false, params Expression<Func<T, object[]>>[] includes)
    {
        var items = FindAll(trackChanges);
        items = includes.Aggregate(items, (cur, include) => cur.Include(include));
        return items;
    }

    public virtual IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false)
    {
        return trackChanges ? _dbSet.Where(expression) : _dbSet.Where(expression).AsNoTracking();
    }

    public virtual IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false,
        params Expression<Func<T, object[]>>[] includes)
    {
        var items = FindByCondition(expression, trackChanges);
        items = includes.Aggregate(items, (cur, include) => cur.Include(include));
        return items;
    }

    public virtual Task<T?> GetByIdAsync(TK id, bool trackChanges = false)
    {
        Expression<Func<T, bool>> func = x => x.Id.Equals(id);
        var result = FindByCondition(func, trackChanges).FirstOrDefaultAsync();
        return result;
    }

    public virtual Task<T?> GetByIdAsync(TK id, bool trackChanges = false,
        params Expression<Func<T, object[]>>[] includes)
    {
        Expression<Func<T, bool>> func = x => x.Id.Equals(id);
        var result = FindByCondition(func, trackChanges, includes).FirstOrDefaultAsync();
        return result;
    }
}