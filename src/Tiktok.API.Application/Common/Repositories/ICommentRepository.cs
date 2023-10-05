using Tiktok.API.Domain.MongoEntities;
using Tiktok.API.Domain.Repositories;

namespace Tiktok.API.Application.Common.Repositories;

public interface ICommentRepository: IMongoRepositoryBase<Comment>
{
    public Task SetHasChildAsync(string commentId, bool hasChild);
    
    public Task<IEnumerable<Comment>> GetRootCommentsByVideoIdAsync(string videoId, int pageIndex, int pageSize);
    
    public Task<long> CountRootCommentsByVideoIdAsync(string videoId);
    
    public Task<IEnumerable<Comment>> GetChildCommentsByRootCommentIdAsync(string rootCommentId, string videoId, int pageIndex, int pageSize);
    
    public Task<long> CountChildCommentsByRootCommentIdAsync(string rootCommentId, string videoId);
}