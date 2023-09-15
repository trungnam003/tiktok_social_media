using MongoDB.Bson.Serialization.Attributes;
#nullable disable
namespace Tiktok.API.Domain.Entities.Abstracts;

public abstract class MongoEntity
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    [BsonElement("_id")]
    public string Id { get; set; }
    
    [BsonElement("createdDate")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    [BsonElement("modifiedDate")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime? ModifiedDate { get; set; }
}