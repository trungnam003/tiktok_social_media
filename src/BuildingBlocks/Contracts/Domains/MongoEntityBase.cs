using Contracts.Domains.Interfaces;
using MongoDB.Bson.Serialization.Attributes;

namespace Contracts.Domains;

public abstract class MongoEntityBase : IMongoEntityBase
{
    [BsonId]
    [BsonElement("_id")]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string Id { get; set; }
    
    [BsonElement("createdDate")]
    [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    [BsonElement("lastModifiedDate")]
    [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
    public DateTime? LastModifiedDate { get; set; }
}