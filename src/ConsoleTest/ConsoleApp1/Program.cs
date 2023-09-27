// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Logging;

using ConsoleApp1;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Linq;
using Tiktok.API.Domain.Entities;
using Tiktok.API.Infrastructure.Persistence;
using Xabe.FFmpeg;

internal class Program
{
    public static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder => {
        // Thiết lập logger
        builder.AddFilter(DbLoggerCategory.Query.Name, LogLevel.Information);

        // builder.AddFilter(DbLoggerCategory.Database.Name, LogLevel.Information);

        builder.AddConsole();

    });
    public static async Task Main(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseLoggerFactory(Program.loggerFactory);
        optionsBuilder.UseSqlServer(
            "Server=localhost,1435;Database=TiktokDb;User=sa;Password=Trungnam.123;MultipleActiveResultSets=True;TrustServerCertificate=true;");
        await using var dbContext = new AppDbContext(optionsBuilder.Options);

        var query =
            dbContext.Users
                .Where(x => x.Id.Equals("e4b49e91-2e04-426f-8dbb-082b067a851f"))
                .Select(u => new
                {
                    Id = u.Id,
                    UserName = u.UserName,

                })
                .Join(dbContext.UserFollowers, user => user.Id, following => following.FollowingId,
                    (user, following) => new
                    {
                        user, following
                    }
                )
                .Join(dbContext.Users, uf => uf.following.FollowerId, user => user.Id,
                    (arg1, user) => new
                    {
                        following = arg1.following,
                        user = arg1.user,
                        follower = user
                    }
                ).Select(x => new
                {
                    x.user,
                    x.following,
                    follower = new
                    {
                        Id= x.follower.Id,
                        UserName = x.follower.UserName
                    }
                });
                
                    
            
                    
            
            // from user in dbContext.Users
            // join userFollower in dbContext.UserFollowers on user.Id equals userFollower.FollowerId
            // join userFollowing in dbContext.Users on userFollower.FollowingId equals userFollowing.Id into Followings
            // where user.Id == "e4b49e91-2e04-426f-8dbb-082b067a851f"
            // select
            //     new
            //     {
            //         user = user,
            //         followings = Followings
            //
            //     };
                
        
        var user1 = await query.ToListAsync();
        
        
        // use linq
        // var query = from user in dbContext.Users
        //     where user.Id == "e4b49e91-2e04-426f-8dbb-082b067a851f"
        //     select user;
        //
        // User? user1 = await query.FirstOrDefaultAsync();
        // var user2 = await dbContext.Users.Where(x => x.UserName.Equals("user3"))
        //     .FirstOrDefaultAsync();
        //
        // await dbContext.UserFollowers.AddAsync(new UserFollower()
        // {
        //     FollowerId = user2.Id,
        //     FollowingId = user1.Id,
        //
        // });
        // await dbContext.SaveChangesAsync();

    }
}