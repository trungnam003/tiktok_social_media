using Contracts.Repositories;
using Tiktok.API.Domain.Entities;

namespace Tiktok.API.Application.Common.Repositories;

public interface IVideoRepository : IRepositoryBase<Video, string>
{
    Task CreateVideoAsync(Video video);
    
}