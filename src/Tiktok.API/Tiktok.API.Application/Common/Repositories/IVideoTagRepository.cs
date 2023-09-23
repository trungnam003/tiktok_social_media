using Contracts.Repositories;
using Tiktok.API.Domain.MongoEntities;

namespace Tiktok.API.Application.Common.Repositories;

public interface IVideoTagRepository : IMongoRepositoryBase<VideoTag>
{
    Task IncreaseTagViewCount(string tagName);
}