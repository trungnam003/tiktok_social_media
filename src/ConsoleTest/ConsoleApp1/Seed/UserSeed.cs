using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Tiktok.API.Domain.Entities;

namespace ConsoleApp1.Seed;

public class UserSeed
{
    public static async Task SeedUser()
    {
        int totalRequests = 10000;
        int requestsPerBatch = 1000;
        string apiUrl = "https://localhost:7125/api/users/register"; // Thay thế bằng URL thực tế của API
        var httpClient = new HttpClient();

        for (int i = 0; i < totalRequests; i += requestsPerBatch)
        {
            var batchTasks = new List<Task>();

            for (int j = 0; j < requestsPerBatch; j++)
            {
                var user = new
                {
                    fullName = "Test User" + (i + j + 1),
                    username = "test" + (i + j + 1),
                    email = "test" + (i + j + 1) + "@example.com",
                    password = "1234",
                    confirmPassword = "1234"
                };

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var task = httpClient.PostAsync(apiUrl, content)
                    .ContinueWith(responseTask =>
                    {
                        if (responseTask.Result.IsSuccessStatusCode)
                        {
                            Console.WriteLine($"Request {i + j + 1} succeeded.");
                        }
                        else
                        {
                            Console.WriteLine($"Request {i + j + 1} failed with status code {responseTask.Result.StatusCode}");
                        }
                    });

                batchTasks.Add(task);
            }

            await Task.WhenAll(batchTasks);
        }

        Console.WriteLine("All requests completed.");
    }

    public static async Task<List<string>> GetTokenTestUser()
    {
        int requestsPerBatch = 1000;
        string apiUrl = "https://localhost:7125/api/users/login"; // Thay thế bằng URL thực tế của API
        var httpClient = new HttpClient();

        var list = new List<string>();

        for (int j = 5000; j < requestsPerBatch + 5000; j++)
        {
            var login = new
            {
                
                email = "test" + (j + 1) + "@example.com",
                password = "1234",
                
            };

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(login);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var task = httpClient.PostAsync(apiUrl, content);

            var result = await task;

            if(result.EnsureSuccessStatusCode().IsSuccessStatusCode)
            {
                var response = await result.Content.ReadAsStringAsync();

                var responseObj = Newtonsoft.Json.JsonConvert.DeserializeObject<Response>(response);

                // System.Console.WriteLine(responseObj.data.token);
                list.Add(responseObj.data.token);
            }

        }

        return list;

    }


    public static async Task FollowUser(List<string> tokens, string id){
        
        string apiUrl = "https://localhost:7125/api/users/follow"; // Thay thế bằng URL thực tế của API
        var httpClient = new HttpClient();

        var tasks = new List<Task>();

        for (int j = 0; j < tokens.Count; j++){
            var token = tokens[j];
            var follow = new
            {
                followingId = id
            };

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(follow);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var task = httpClient.PostAsync(apiUrl, content);

            tasks.Add(task);
            
        }

        await Task.WhenAll(tasks);

    }

    public class Response{
        public Data data {get;set;}
    }

    public class Data{
        public string token {get;set;}
    }
}