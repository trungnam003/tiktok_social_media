using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;
using Serilog;
using Serilog.Core;

namespace ConsoleApp1.Mongo;

public class Handler 
{
    public static async Task HandlerMongo()
    {
        var logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();
        var connectionString = "mongodb://root:Trungnam.123@localhost:27018/TestDb?authSource=admin";
        var mongoConnectionUrl = new MongoUrl(connectionString);
        var mongoClientSettings = MongoClientSettings.FromUrl(mongoConnectionUrl);
        // mongoClientSettings.ClusterConfigurator = cb => {
        //     cb.Subscribe<CommandStartedEvent>(e => {
        //         logger.Information($"{e.CommandName} - {e.Command.ToJson()}");
        //     });
        // };
        var client = new MongoClient(mongoClientSettings);
        await client.StartSessionAsync();
        
        var database = client.GetDatabase("TestDb");
        var collection = database.GetCollection<Comment>("comments");
        var indexDefine1 = Builders<Comment>.IndexKeys.Ascending(x => x.VideoId);
        var indexOptions1 = new CreateIndexOptions { Unique = false, Name = "VideoIdIndex"};
        var indexModel1 = new CreateIndexModel<Comment>(indexDefine1, indexOptions1);
        await collection.Indexes.CreateOneAsync(indexModel1);
        
        var indexDefine2 = Builders<Comment>.IndexKeys.Ascending(x => x.RootId);
        var indexOptions2 = new CreateIndexOptions { Unique = false,Name = "ParentIdIndex"};
        var indexModel2 = new CreateIndexModel<Comment>(indexDefine2, indexOptions2);
        await collection.Indexes.CreateOneAsync(indexModel2);

        var q = await collection.FindAsync(x => x.Id.Equals("651b8c710233f727e4cfd811"));
        
        var commentReply = q.FirstOrDefault();

        Console.WriteLine(commentReply?.Id);
        //
        // // bulk insert
        // var list = new List<Comment>();
        // for (var i = 1; i <=1000; i++)
        // {
        //     var comment = new Comment()
        //     {
        //         Content = "test comment" + i,
        //         VideoId = "video_123",
        //         UserId = "user_321",
        //         IsRoot = false,
        //         RootId = commentReply.Id,
        //         RelyUserId = "user_321",
        //         ReactionCount = 0,
        //         CreatedDate = DateTime.UtcNow,
        //         LastModifiedDate = DateTime.UtcNow,
        //         HasChild = false
        //     };
        //     list.Add(comment);
        // }
        //
        // await collection.UpdateOneAsync(x => x.Id.Equals(commentReply.Id), Builders<Comment>.Update.Set(x => x.HasChild, true));
        // //
        // await collection.InsertManyAsync(list);
        
        // paging
        // calculate time handle
        var watch = System.Diagnostics.Stopwatch.StartNew();
        
        // var filter = Builders<Comment>.Filter.Eq(x => x.VideoId, "video_123")
        //              & Builders<Comment>.Filter.Eq(x => x.RootId, "651b8c710233f727e4cfd811");
        // var sort = Builders<Comment>.Sort.Ascending(x => x.CreatedDate);
        // var options = new FindOptions<Comment, Comment>()
        // {
        //     Sort = sort,
        //     Limit = 10,
        //     Skip = 0
        // };
        // var result = await collection.FindAsync(filter, options);
        // var list = result.ToList();
        // var count = await collection.CountDocumentsAsync(x => x.RootId.Equals("651b8c710233f727e4cfd811"));
        // Console.WriteLine(count);
        // foreach (var item in list)
        // {
        //     Console.WriteLine(item.Content);
        // }
        
        // var query = from comment in collection.AsQueryable()
        //     where comment.VideoId == "video_123" && comment.IsRoot
        //     select comment;
        //
        // var list1 = query.Skip(0).Take(10).ToList();
        //
        // foreach (var items in list1)
        // {
        //     if (items.HasChild)
        //     {
        //         // count child
        //         var count = await collection.CountDocumentsAsync(x => x.RootId.Equals(items.Id));
        //         Console.WriteLine(count);
        //     }
        //
        //     Console.WriteLine(items.Content);
        // }
        //
        
        
        watch.Stop();
        var elapsedMs = watch.ElapsedMilliseconds;
        Console.WriteLine(elapsedMs);
        
        
        // get comment
        // var q = from comments in collection.AsQueryable()
        //     join childComment in collection.AsQueryable() on comments.Id equals childComment.ParentId into childComments
        //     where comments.VideoId == "video123" && comments.IsParent
        //     select new
        //     {
        //         comments,
        //         childComments
        //     };
        //
        // var result = q.ToList();
        // foreach (var item in result)
        // {
        //     Console.WriteLine(item.comments.Content);
        //     foreach (var childComment in item.childComments)
        //     {
        //         Console.WriteLine("\t" + childComment.Content);
        //     }
        // }

        // // multi filter
        // var comment = collection.Aggregate()
        //     .Match(x => x.IsParent && x.VideoId == "video123")
        //     .Lookup("comments", "_id", "parentId", "ChildComments");
        //
        // var result = comment.ToList();
        // foreach (var item in result)
        // {
        //     // deserialize to object
        //     var comment1 = BsonSerializer.Deserialize<Comment>(item);
        //     Console.WriteLine(comment1.Content);
        // }
        // var listId = new List<string>()
        // {
        //     "651a371b5a195484a3cbcad4",
        //     "651a37c72953bbbc20f8eb0f"
        // };
        // var data = collection.AsQueryable()
        //     .Where(x => listId.Contains(x.Id))
        //     .ToList();
        //
        // var andFilter = Builders<Comment>.Filter.Lt(x => x.CreatedDate, DateTime.UtcNow.AddDays(-1));
        // var test = await collection.FindAsync(andFilter);


    }
}