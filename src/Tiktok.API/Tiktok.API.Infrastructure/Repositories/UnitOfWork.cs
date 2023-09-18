using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Tiktok.API.Domain.Repositories;

namespace Tiktok.API.Infrastructure.Repositories;

public class UnitOfWork<TContext> : IUnitOfWork<TContext>
    where TContext : DbContext
{
    private readonly DbContext _context;
    private bool _disposed;

    public UnitOfWork(DbContext context, IDbContextTransaction transaction)
    {
        _context = context;
        _disposed = false;
    }

    public Task<IDbContextTransaction> BeginTransactionAsync()
    {
        CheckDisposed();
        return _context.Database.BeginTransactionAsync();
    }

    public Task EndTransactionAsync()
    {
        CheckDisposed();
        return _context.Database.CommitTransactionAsync();
    }

    public Task RollbackTransactionAsync()
    {
        CheckDisposed();
        return _context.Database.RollbackTransactionAsync();
    }

    public Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        CheckDisposed();
        return _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void CheckDisposed()
    {
        if (_disposed)
            throw new ObjectDisposedException(GetType().Name);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
            if (disposing)
                _context.Dispose();
        _disposed = true;
    }
}