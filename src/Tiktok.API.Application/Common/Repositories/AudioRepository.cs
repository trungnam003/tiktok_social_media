using Tiktok.API.Domain.Entities;
using Tiktok.API.Domain.Repositories;

namespace Tiktok.API.Application.Common.Repositories;

public interface IAudioRepository : IRepositoryBase<Audio, string> 
{
    Task CreateAudioAsync(Audio audio);
}