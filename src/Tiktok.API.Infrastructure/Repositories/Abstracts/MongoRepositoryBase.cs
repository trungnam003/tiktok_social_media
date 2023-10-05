using System.Linq.Expressions;
using MongoDB.Driver;
using Tiktok.API.Domain.Attributes;
using Tiktok.API.Domain.Configurations;
using Tiktok.API.Domain.Entities.Abstracts;
using Tiktok.API.Domain.Repositories;

namespace Tiktok.API.Infrastructure.Repositories.Abstracts;

public abstract class MongoRepositoryBase<T> : IMongoRepositoryBase<T>
    where T : MongoEntityBase
{
    private readonly IMongoDatabase _database;

    protected MongoRepositoryBase(IMongoClient client, MongoDbDatabaseSettings databaseSettings)
    {
        _database = client.GetDatabase(databaseSettings.DatabaseName);
    }

    protected virtual IMongoCollection<T> Collection =>
        _database.GetCollection<T>(GetCollectionName());

    private static string GetCollectionName()
    {
        var collectionName = (typeof(T).GetCustomAttributes(typeof(BsonCollectionAttribute), true)
            .FirstOrDefault() as BsonCollectionAttribute)?.CollectionName;
        return collectionName!;
    }

    public Task AddAsync(T entity)
    {
        return Collection.InsertOneAsync(entity);
    }

    public Task<T> FindOneAsync(Expression<Func<T, bool>> predicate)
    {
        return Collection.Find(predicate).FirstOrDefaultAsync();
    }

    public Task UpdateAsync(T entity)
    {
        return Collection.ReplaceOneAsync(x => x.Id == entity.Id, entity);
    }

    public Task DeleteAsync(string id)
    {
        return Collection.DeleteOneAsync(x => x.Id == id);
    }
}