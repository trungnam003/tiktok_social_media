using Contracts.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Repositories.Interfaces;

public interface IUnitOfWork<TContext> : IDisposable, IUnitOfWorkBase 
    where TContext : DbContext
{
    Task<IDbContextTransaction> BeginTransactionAsync();
    Task EndTransactionAsync();
    Task RollbackTransactionAsync();
}