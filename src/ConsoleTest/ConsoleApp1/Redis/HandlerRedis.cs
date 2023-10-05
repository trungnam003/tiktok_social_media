using StackExchange.Redis;
using Tiktok.API.Infrastructure.Services;

namespace ConsoleApp1.Redis;

public class HandlerRedis
{
    
    public static async Task Test()
    {
        IConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:6379,password=Trungnam.123");
        IDatabase db = redis.GetDatabase();
        var key = "key";
        var jsonService = new SerializeService();
        // set with expire Absolute Expiration and Sliding Expiration 
        // await db.StringSetAsync(key, json, TimeSpan.FromSeconds(10));
        
        // pipeline set multiple key
        var batch = db.CreateBatch();
        var tasks = new List<Task>();
        for (int i = 1; i <= 100; i++)
        {
            var user = new User()
            {
                Age = i + 1,
                Name = $"Trung Nam {i}",
                Email = $"email {i}"
            };
            var json = jsonService.Serialize(user);
            tasks.Add(batch.StringSetAsync($"key{i}", json, TimeSpan.FromSeconds(20)));
        }
        
        batch.Execute();
        await Task.WhenAny(tasks);
        
        // batch get
        var batchGet = db.CreateBatch();
        var tasksGet = new List<Task<RedisValue>>();
        for (int i = 1; i <= 100; i++)
        {
            tasksGet.Add(batchGet.StringGetAsync($"key{i}"));
        }
        batchGet.Execute();
        var result = await Task.WhenAll(tasksGet);
        foreach (var item in result)
        {
            Console.WriteLine(jsonService.Deserialize<User>(item));
        }
    }
}
class User
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }

    public override string ToString()
    {
        return $"Name: {Name}, Age: {Age}, Email: {Email}";
    }
}