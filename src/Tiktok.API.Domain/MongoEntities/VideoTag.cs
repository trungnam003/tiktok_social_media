using MongoDB.Bson.Serialization.Attributes;
using Tiktok.API.Domain.Attributes;
using Tiktok.API.Domain.Entities.Abstracts;

namespace Tiktok.API.Domain.MongoEntities;
#nullable disable
[BsonCollection("VideoTags")]
public class VideoTag : MongoEntityBase
{
    [BsonElement("tagName")] 
    public string TagName { get; set; }
    [BsonElement("totalView")] 
    public long TotalView { get; set; }
}