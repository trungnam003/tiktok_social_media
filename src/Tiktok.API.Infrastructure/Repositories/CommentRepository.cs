using MongoDB.Driver;
using Tiktok.API.Application.Common.Repositories;
using Tiktok.API.Domain.Configurations;
using Tiktok.API.Domain.MongoEntities;
using Tiktok.API.Infrastructure.Repositories.Abstracts;

namespace Tiktok.API.Infrastructure.Repositories;

public class CommentRepository : MongoRepositoryBase<Comment> , ICommentRepository
{
    public CommentRepository(IMongoClient client, MongoDbDatabaseSettings databaseSettings) : base(client, databaseSettings)
    {
    }

    public Task SetHasChildAsync(string commentId, bool hasChild)
    {
        var filter = Builders<Comment>.Filter.Eq(x => x.Id, commentId);
        var update = Builders<Comment>.Update.Set(x => x.HasChild, hasChild);
        return Collection.UpdateOneAsync(filter, update);
    }

    public Task<IEnumerable<Comment>> GetRootCommentsByVideoIdAsync(string videoId, int pageIndex, int pageSize)
    {
        var q = from comment in Collection.AsQueryable()
            join childComment in Collection.AsQueryable() on comment.Id equals childComment.RootId into childComments
            where comment.VideoId.Equals(videoId) && comment.IsRoot
            orderby comment.CreatedDate descending
            select new
            {
                comment,
                childCount = childComments.Count()
            };
        
        var result = q.Skip((pageIndex-1)*pageSize).Take(pageSize).ToList();
        foreach (var item in result)
        {
            item.comment.CountChild = item.childCount;
        }
        
        return Task.FromResult(result.Select(x => x.comment));
        
        // var filter = Builders<Comment>.Filter.Eq(x => x.VideoId, videoId)
        //              & Builders<Comment>.Filter.Eq(x => x.IsRoot, true);
        //
        // var options = new FindOptions<Comment, Comment>()
        // {
        //     Limit = pageSize,
        //     Skip = (pageIndex-1) * pageSize,
        //     Sort =  Builders<Comment>.Sort.Descending(x => x.CreatedDate)
        // };
        //
        // var result = await Collection.FindAsync(filter, options);
        // return result.ToList();
    }

    public Task<long> CountRootCommentsByVideoIdAsync(string videoId)
    {
        var filter = Builders<Comment>.Filter.Eq(x => x.VideoId, videoId)
            & Builders<Comment>.Filter.Eq(x => x.IsRoot, true);
         
        return Collection.CountDocumentsAsync(filter);
    }

    public async Task<IEnumerable<Comment>> GetChildCommentsByRootCommentIdAsync(string rootCommentId, string videoId, int pageIndex, int pageSize)
    {
        var filter = Builders<Comment>.Filter.Eq(x => x.RootId, rootCommentId)
                     & Builders<Comment>.Filter.Eq(x => x.IsRoot, false)
                     & Builders<Comment>.Filter.Eq(x => x.VideoId, videoId);
        var options = new FindOptions<Comment, Comment>()
        {
            Skip = (pageIndex - 1) * pageSize,
            Limit = pageSize,
            Sort = Builders<Comment>.Sort.Descending(x => x.CreatedDate)
        };
        var result = await Collection.FindAsync(filter, options);
        return result.ToList();
    }

    public Task<long> CountChildCommentsByRootCommentIdAsync(string rootCommentId, string videoId)
    {
        var filter = Builders<Comment>.Filter.Eq(x => x.RootId, rootCommentId)
                     & Builders<Comment>.Filter.Eq(x => x.IsRoot, false)
                     & Builders<Comment>.Filter.Eq(x => x.VideoId, videoId);
        return Collection.CountDocumentsAsync(filter);
    }

    public async Task<bool> DeleteCommentAsync(string commentId, string userId)
    {
        var filter = Builders<Comment>.Filter.Eq(x => x.Id, commentId);
        
        var findResult = await Collection.FindAsync(filter);
        var comment = findResult.FirstOrDefault();
        if (comment == null || !comment.UserId.Equals(userId))
        {
            return false;
        }
        if (comment.HasChild)
        {
            // delete all child comments
            var deleteChildFilter = Builders<Comment>.Filter.Eq(x => x.RootId, comment.Id);
            await Collection.DeleteManyAsync(deleteChildFilter);
        }
        // delete comment
        await Collection.DeleteOneAsync(filter);
        return true;
    }

    public async Task DeleteCommentAsync(string videoId)
    {
        var filter = Builders<Comment>.Filter.Eq(x => x.VideoId, videoId);
        await Collection.DeleteManyAsync(filter);
    }
}