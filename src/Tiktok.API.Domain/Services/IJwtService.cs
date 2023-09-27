using Tiktok.API.Domain.Entities.Interfaces;

namespace Tiktok.API.Domain.Services;

public interface IJwtService<in T> where T : IEntityBase<string>
{
    Task<string> CreateToken(T user);
}