using Microsoft.EntityFrameworkCore;

namespace Tiktok.API.Domain.Common.Repositories;

public interface IUnitOfWork<TContext> : IDisposable where TContext : DbContext
{
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}