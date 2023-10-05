using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Tiktok.API.Domain.Attributes;
using Tiktok.API.Domain.Configurations;
using Tiktok.API.Domain.Entities.Interfaces;
using Tiktok.API.Domain.MongoEntities;
using IHost = Microsoft.Extensions.Hosting.IHost;


namespace Tiktok.API.Infrastructure;

public static class EntityMongoBuilder
{
    public static void ConfigureEntityMongoBuilder(this IHost host)
    {
        // get service
        using var scope = host.Services.CreateScope();
        
        var provider = scope.ServiceProvider;
        var mongoClient = provider.GetRequiredService<IMongoClient>();
        var settings = provider.GetRequiredService<MongoDbDatabaseSettings>();
        
        var database = mongoClient.GetDatabase(settings.DatabaseName);
        CreateIndexVideoTag(mongoClient, database);
        CreateIndexComment(mongoClient, database);
    }

    private static void CreateIndexVideoTag(IMongoClient mongoClient, IMongoDatabase database)
    {
        var collectionName = GetCollectionName<VideoTag>();
        var tagsCollection = database.GetCollection<VideoTag>(collectionName);
        var indexDefinition = Builders<VideoTag>.IndexKeys.Ascending(x => x.TagName);
        var indexOptions = new CreateIndexOptions { Unique = true };
        var indexModel = new CreateIndexModel<VideoTag>(indexDefinition, indexOptions);
        tagsCollection.Indexes.CreateOne(indexModel);
    }

    private static void CreateIndexComment(IMongoClient mongoClient, IMongoDatabase database)
    {
        var collectionName = GetCollectionName<Comment>();
        var commentsCollection = database.GetCollection<Comment>(collectionName);
        
        var indexVideoIdDefine = Builders<Comment>.IndexKeys.Ascending(x => x.VideoId);
        var indexVideoIdOptions = new CreateIndexOptions { Unique = false, Name = "video_id_index"};
        var indexVideoIdModel = new CreateIndexModel<Comment>(indexVideoIdDefine, indexVideoIdOptions);
        commentsCollection.Indexes.CreateOne(indexVideoIdModel);
        
        var indexRootIdDefine = Builders<Comment>.IndexKeys.Ascending(x => x.RootId);
        var indexRootIdOptions = new CreateIndexOptions { Unique = false,Name = "root_id_index"};
        var indexRootIdModel = new CreateIndexModel<Comment>(indexRootIdDefine, indexRootIdOptions);
        commentsCollection.Indexes.CreateOne(indexRootIdModel);
    }
    
    private static string GetCollectionName<T>()
        where T : IMongoEntityBase
    {
        var result = (typeof(T).GetCustomAttributes(typeof(BsonCollectionAttribute), inherit: true)
            .FirstOrDefault() as BsonCollectionAttribute)?.CollectionName;
        if (string.IsNullOrEmpty(result))
        {
            throw new Exception("Collection name could not be inferred");
        }

        return result;
    }
}