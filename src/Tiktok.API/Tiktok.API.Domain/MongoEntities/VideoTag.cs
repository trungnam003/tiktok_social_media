using Contracts.Domains;
using Contracts.Mongo;
using MongoDB.Bson.Serialization.Attributes;

namespace Tiktok.API.Domain.MongoEntities;

[BsonCollection("VideoTags")]
public class VideoTag : MongoEntityBase
{
    [BsonElement("tagName")] 
    public string TagName { get; set; }
    [BsonElement("totalView")] 
    public long TotalView { get; set; }
}