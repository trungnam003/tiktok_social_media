using Infrastructure.Configurations;
using Infrastructure.Repositories;
using MongoDB.Driver;
using Tiktok.API.Application.Common.Repositories;
using Tiktok.API.Domain.MongoEntities;

namespace Tiktok.API.Infrastructure.Repositories;

public class VideoTagRepository : MongoRepositoryBase<VideoTag>, IVideoTagRepository
{
    public VideoTagRepository(IMongoClient client, MongoDbDatabaseSettings databaseSettings) : base(client, databaseSettings)
    {
    }

    public Task IncreaseTagViewCount(string tagName)
    {
        // increase if exists, otherwise create
        var filter = Builders<VideoTag>.Filter.Eq(x => x.TagName, tagName);
        var update = Builders<VideoTag>.Update
            .SetOnInsert(x => x.CreatedDate, DateTime.UtcNow)
            .Set(x => x.LastModifiedDate, DateTime.UtcNow)
            .SetOnInsert(x => x.TagName, tagName)
            // increase if exists, otherwise create
            .Inc(x => x.TotalView, 1);
        return Collection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true,  });
    }
}