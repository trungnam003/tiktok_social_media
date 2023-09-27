
using MongoDB.Bson.Serialization.Attributes;
using Tiktok.API.Domain.Entities.Interfaces;

namespace Tiktok.API.Domain.Entities.Abstracts;

public abstract class MongoEntityBase : IMongoEntityBase
{
    [BsonId]
    [BsonElement("_id")]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string Id { get; set; }
}