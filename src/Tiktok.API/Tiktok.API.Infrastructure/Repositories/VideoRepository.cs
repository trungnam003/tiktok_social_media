using Infrastructure.Repositories;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Tiktok.API.Application.Common.Repositories;
using Tiktok.API.Domain.Entities;
using Tiktok.API.Infrastructure.Persistence;

namespace Tiktok.API.Infrastructure.Repositories;

public class VideoRepository : RepositoryQueryCommand<Video, string, AppDbContext>, IVideoRepository
{
    private readonly AppDbContext _context;
    public VideoRepository(AppDbContext context, IUnitOfWork<AppDbContext> unitOfWork) : base(context, unitOfWork)
    {
        _context = context;
    }

    public async Task CreateVideoAsync(Video video)
    {
        await CreateAsync(video);

        await SaveChangesAsync();
    }
    
}