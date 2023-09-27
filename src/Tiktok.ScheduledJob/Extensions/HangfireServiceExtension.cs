using System.Security.Authentication;
using Hangfire;
using Hangfire.Console;
using Hangfire.Console.Extensions;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using MongoDB.Driver;
using Newtonsoft.Json;
using Tiktok.API.Domain.Configurations;

namespace Tiktok.ScheduledJob.Extensions;

public static class HangfireServiceExtension
{
    internal static void AddHangfireService(this IServiceCollection services, IConfiguration configuration)
    {
        var hangfireSettings = configuration.GetSection(nameof(HangfireSettings))
            .Get<HangfireSettings>();
        if (hangfireSettings == null)
        {
            throw new ArgumentNullException(nameof(hangfireSettings));
        }
        
        services.ConfigureHangFireServices(hangfireSettings);
        services.AddHangfireServer(serverOptions =>
        {
            serverOptions.ServerName = hangfireSettings.ServerName;
        });
    }

    private static IServiceCollection ConfigureHangFireServices(this IServiceCollection services,
        HangfireSettings settings)
    {
        var mongoUrlBuilder = new MongoUrlBuilder(settings.Storage.ConnectionString);
        var mongoClientSettings = MongoClientSettings.FromUrl(mongoUrlBuilder.ToMongoUrl());

        mongoClientSettings.SslSettings = new SslSettings
        {
            EnabledSslProtocols = SslProtocols.Tls12
        };
        
        var mongoClient = new MongoClient(mongoClientSettings);
        var mongoStorageOptions = new MongoStorageOptions
        {
            MigrationOptions = new MongoMigrationOptions()
            {
                MigrationStrategy = new MigrateMongoMigrationStrategy(),
                BackupStrategy = new CollectionMongoBackupStrategy()
            },
            CheckConnection = true,
            Prefix = "SchedulerQueue",
            CheckQueuedJobsStrategy = CheckQueuedJobsStrategy.TailNotificationsCollection
        };
        
       // add hangfire service with mongo storage
       services.AddHangfire((provider, config) =>
       {
           config.UseSimpleAssemblyNameTypeSerializer()
               .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
               .UseRecommendedSerializerSettings()
               .UseConsole()
               .UseMongoStorage(mongoClient, mongoUrlBuilder.DatabaseName, mongoStorageOptions);

           var jsonSettings = new JsonSerializerSettings
           {
               TypeNameHandling = TypeNameHandling.All
           };

           config.UseSerializerSettings(jsonSettings);

       });
       services.AddHangfireConsoleExtensions();
       return services;
    }
}