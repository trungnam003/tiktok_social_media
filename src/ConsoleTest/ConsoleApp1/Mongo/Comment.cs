using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Tiktok.API.Domain.Entities.Abstracts;

namespace ConsoleApp1.Mongo;
#nullable disable
[BsonIgnoreExtraElements]
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
    public bool HasChild { get; set; }
    
    [BsonElement("replyUserId")]
    public string? RelyUserId { get; set; }
    
    [BsonElement("reactionCount")]
    public int ReactionCount { get; set; }
    
}