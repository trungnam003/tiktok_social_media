using Contracts.Domains.Interfaces;
using Contracts.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Interfaces;

public interface IRepositoryCommand<T, TK, TContext> : IRepositoryCommandBase<T, TK>
    where T : IEntityBase<TK>
    where TContext : DbContext
{
    IUnitOfWork<TContext> UnitOfWork { get; }
}