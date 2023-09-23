using Infrastructure.Repositories;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Tiktok.API.Application.Common.Repositories;
using Tiktok.API.Domain.Entities;
using Tiktok.API.Infrastructure.Persistence;

namespace Tiktok.API.Infrastructure.Repositories;

public class AudioRepository : RepositoryQueryCommand<Audio, string, AppDbContext>, IAudioRepository
{
    public AudioRepository(AppDbContext context, IUnitOfWork<AppDbContext> unitOfWork) : base(context, unitOfWork)
    {
    }

    public async Task CreateAudioAsync(Audio audio)
    {
        await CreateAsync(audio);
        await SaveChangesAsync();
    }
}