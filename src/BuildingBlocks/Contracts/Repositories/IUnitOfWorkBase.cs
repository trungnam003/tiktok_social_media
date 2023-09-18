namespace Contracts.Repositories;

public interface IUnitOfWorkBase: IDisposable
{
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}