using Contracts.Domains.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Tiktok.API.Domain.Repositories;


public interface IRepositoryQuery<T,in TK, TContext> : IRepositoryQueryBase<T, TK>
    where T : IEntityBase<TK>
    where TContext : DbContext
{
    
}