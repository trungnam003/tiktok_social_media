using System.Linq.Expressions;
using Contracts.Domains;
using Contracts.Mongo;
using Contracts.Repositories;
using Infrastructure.Configurations;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public abstract class MongoRepositoryBase<T> : IMongoRepositoryBase<T>
    where T : MongoEntityBase
{
    private readonly IMongoDatabase _database;

    public MongoRepositoryBase(IMongoClient client, MongoDbDatabaseSettings databaseSettings)
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