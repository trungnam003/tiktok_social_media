using Contracts.Domains.Interfaces;
using Microsoft.EntityFrameworkCore;
using Contracts.Repositories;

namespace Infrastructure.Repositories.Interfaces;


public interface IRepositoryQuery<T,TK, TContext> : IRepositoryQueryBase<T, TK>
    where T : IEntityBase<TK>
    where TContext : DbContext
{
    
}