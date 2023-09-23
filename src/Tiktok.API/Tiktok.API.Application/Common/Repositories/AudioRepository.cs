using Contracts.Repositories;
using Tiktok.API.Domain.Entities;

namespace Tiktok.API.Application.Common.Repositories;

public interface IAudioRepository : IRepositoryBase<Audio, string> 
{
    Task CreateAudioAsync(Audio audio);
}