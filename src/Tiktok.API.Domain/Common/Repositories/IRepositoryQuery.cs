using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Tiktok.API.Domain.Entities.Interfaces;

namespace Tiktok.API.Domain.Common.Repositories;

public interface IRepositoryQuery<T, TK> 
    where T : IEntityBase<TK>
{
    IQueryable<T> FindAll(bool trackChanges = false);
    IQueryable<T> FindAll(bool trackChanges = false, params Expression<Func<T, object[]>> [] includes);
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false);
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false, params Expression<Func<T, object[]>> [] includes);
    
    Task<T?> GetByIdAsync(TK id, bool trackChanges = false);
    Task<T?> GetByIdAsync(TK id, bool trackChanges = false, params Expression<Func<T, object[]>> [] includes);
}

public interface IRepositoryQuery<T, TK, TContext>
    where T : IEntityBase<TK>
    where TContext : DbContext
{
    // No Implementation
}