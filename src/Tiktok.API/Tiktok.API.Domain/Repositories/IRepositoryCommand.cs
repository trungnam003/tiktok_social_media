using Contracts.Domains.Interfaces;
using Contracts.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Tiktok.API.Domain.Repositories;

public interface IRepositoryCommand<in T, TK, TContext> : IRepositoryCommandBase<T, TK>
    where T : IEntityBase<TK>
    where TContext : DbContext
{
    IUnitOfWork<TContext> UnitOfWork { get; }
}