using Contracts.Domains.Interfaces;
using Tiktok.API.Domain.Repositories;

namespace Contracts.Repositories;

public interface IRepositoryBase<T, TK> : IRepositoryCommandBase<T, TK>, IRepositoryQueryBase<T, TK>
    where T : IEntityBase<TK>
{
    
}