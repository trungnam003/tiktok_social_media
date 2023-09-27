
using MongoDB.Bson.Serialization.Attributes;
using Tiktok.API.Domain.Entities.Interfaces;

namespace Tiktok.API.Domain.Entities.Abstracts;

public abstract class MongoEntityAuditableBase : MongoEntityBase, IMongoDateTracking
{
    [BsonElement("createdDate")]
    [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
    public DateTime CreatedDate { get; set; }
    
    [BsonElement("lastModifiedDate")]
    [BsonDateTimeOptions(Kind = System.DateTimeKind.Utc)]
    public DateTime? LastModifiedDate { get; set; }
}