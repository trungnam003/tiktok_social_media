using Contracts.Domains.Interfaces;

namespace Contracts.Services;

public interface IJwtService<in T> where T : IEntityBase<string>
{
    Task<string> CreateToken(T user);
}