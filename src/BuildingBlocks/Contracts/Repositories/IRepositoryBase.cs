using Contracts.Domains.Interfaces;

namespace Contracts.Repositories;

public interface IRepositoryBase<T, TK> : IRepositoryCommandBase<T, TK>, IRepositoryQueryBase<T, TK>
    where T : IEntityBase<TK>
{
    
}