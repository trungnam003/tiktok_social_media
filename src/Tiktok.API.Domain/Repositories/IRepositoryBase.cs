using Tiktok.API.Domain.Entities.Interfaces;
namespace Tiktok.API.Domain.Repositories;

public interface IRepositoryBase<T, TK> : IRepositoryCommandBase<T, TK>, IRepositoryQueryBase<T, TK>
    where T : IEntityBase<TK>
{
    
}