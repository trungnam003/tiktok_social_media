
using MongoDB.Driver;
using Tiktok.API.Application.Common.Repositories;
using Tiktok.API.Domain.Configurations;
using Tiktok.API.Domain.MongoEntities;

namespace Tiktok.API.Infrastructure.Repositories;

public class VideoTagRepository : Abstracts.MongoRepositoryBase<VideoTag>, IVideoTagRepository
{
    public VideoTagRepository(IMongoClient client, MongoDbDatabaseSettings databaseSettings) : base(client, databaseSettings)
    {
    }

    public async Task<BulkWriteResult<VideoTag>?> BulkCreateVideoTagIfNotExist(List<string> tags)
    {
        if(tags.Count == 0) return null;
        var bulkOps = new List<WriteModel<VideoTag>>();
        foreach (var item in tags)
        {
            var filter = Builders<VideoTag>.Filter.Eq(x => x.TagName, item);
            
            var upsertOne = new UpdateOneModel<VideoTag>(filter, 
                Builders<VideoTag>.Update
                    .SetOnInsert(x => x.TagName, item)
                    .SetOnInsert(x=>x.TotalView, 0))
            {
                IsUpsert = true
            };
            bulkOps.Add(upsertOne);
        }

        return await Collection.BulkWriteAsync(bulkOps);   
    }
}