using MongoDB.Bson.Serialization.Attributes;
using Tiktok.API.Domain.Attributes;
using Tiktok.API.Domain.Entities.Abstracts;

namespace Tiktok.API.Domain.MongoEntities;

[BsonCollection("Comments")]
public class Comment : MongoEntityAuditableBase
{
    [BsonElement("content")]
    public string Content { get; set; } = null!;
    
    [BsonElement("videoId")]
    public string VideoId { get; set; } = null!;
    
    [BsonElement("userId")]
    public string UserId { get; set; } = null!;
    
    [BsonElement("isRoot")]
    public bool IsRoot { get; set; }
    
    [BsonElement("rootId")]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string? RootId { get; set; }
    
    [BsonElement("hasChild")]
    [BsonDefaultValue(false)]
    public bool HasChild { get; set; }
    
    [BsonElement("replyUserId")]
    [BsonDefaultValue(null)]
    public string? RelyUserId { get; set; }
    
    [BsonElement("reactionCount")]
    [BsonDefaultValue(0)]
    public int ReactionCount { get; set; }
    
    [BsonIgnore]
    public int CountChild { get; set; }
}