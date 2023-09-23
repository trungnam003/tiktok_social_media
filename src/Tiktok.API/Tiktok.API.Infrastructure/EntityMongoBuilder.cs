using Infrastructure.Configurations;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
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
        var tagsCollection = database.GetCollection<VideoTag>($"{nameof(VideoTag)}"+"s");
        var indexDefinition = Builders<VideoTag>.IndexKeys.Ascending(x => x.TagName);
        var indexOptions = new CreateIndexOptions { Unique = true };
        var indexModel = new CreateIndexModel<VideoTag>(indexDefinition, indexOptions);
        tagsCollection.Indexes.CreateOne(indexModel);
    }
}