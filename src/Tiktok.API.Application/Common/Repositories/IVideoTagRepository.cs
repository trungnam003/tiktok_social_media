
using MongoDB.Driver;
using Tiktok.API.Domain.MongoEntities;
using Tiktok.API.Domain.Repositories;

namespace Tiktok.API.Application.Common.Repositories;

public interface IVideoTagRepository : IMongoRepositoryBase<VideoTag>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="tags">tags is string able split, sep ","</param>
    /// <returns></returns>
    Task<BulkWriteResult<VideoTag>?> BulkCreateVideoTagIfNotExist(List<string> tags);
}