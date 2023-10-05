
using Tiktok.API.Domain.Entities;
using Tiktok.API.Domain.Repositories;

namespace Tiktok.API.Application.Common.Repositories;

public interface IVideoRepository : IRepositoryBase<Video, string>
{
    Task CreateVideoAsync(Video video);
    
    Task<bool> VideoExistsAsync(string videoId);
    
    
}