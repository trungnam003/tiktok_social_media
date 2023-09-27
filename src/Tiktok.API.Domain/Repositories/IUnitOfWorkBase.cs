using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Tiktok.API.Domain.Repositories;

public interface IUnitOfWorkBase: IDisposable
{
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}

public interface IUnitOfWork<TContext> : IDisposable, IUnitOfWorkBase 
    where TContext : DbContext
{
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task EndTransactionAsync();
    Task RollbackTransactionAsync();
}