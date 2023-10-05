using System.Text;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;
using RabbitMQ.Client;
using StackExchange.Redis;
using Tiktok.API.Application.Common.Repositories;
using Tiktok.API.Domain.Configurations;
using Tiktok.API.Domain.Entities;
using Tiktok.API.Domain.EventBusMessages.Events;
using Tiktok.API.Domain.Repositories;
using Tiktok.API.Domain.Services;
using Tiktok.API.Infrastructure.Persistence;
using Tiktok.API.Infrastructure.Repositories;
using Tiktok.API.Infrastructure.Services;
using OtpService = Tiktok.API.Infrastructure.Services.OtpService;

namespace Tiktok.API.Infrastructure;

public static class ConfigureServices
{
    public static void AddInfrastructureLayerServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEntityFramework(configuration);
        services.AddIdentityServices(configuration);
        services.AddMasstransitConfiguration(configuration);
        services.AddRedis(configuration);
        services.AddMongoDb(configuration);

        services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IVideoRepository, VideoRepository>();
        services.AddScoped<IVideoTagRepository, VideoTagRepository>();
        services.AddScoped<IAudioRepository, AudioRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();


        services.AddScoped<IJwtService<User>, JwtService>();
        services.AddScoped<IPublishMessageService, PublishMessageService>();
        services.AddScoped<IOtpService, OtpService>();
        services.AddScoped<ISerializeService, SerializeService>();

        var diskSettings = configuration.GetSection(nameof(DiskStorageSettings))
            .Get<DiskStorageSettings>();
        if (diskSettings == null)
            throw new ArgumentNullException(nameof(diskSettings));
        services.AddSingleton(diskSettings);
        services.AddScoped<IFileService, FileService>();
    }

    private static void AddEntityFramework(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseSettings = configuration.GetSection(nameof(DatabaseSettings))
            .Get<DatabaseSettings>();
        if (databaseSettings == null)
            throw new ArgumentNullException(nameof(databaseSettings));
        services.AddSingleton(databaseSettings);
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(databaseSettings.ConnectionString,
                sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                    // sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                });
        });
    }

    private static void AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options =>
        {
            // Password settings.
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequiredLength = 3;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;

            // Lockout settings.
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            // User settings.
            options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = true;
        });

        var jwtSettings = configuration.GetSection(nameof(JwtSettings))
            .Get<JwtSettings>();
        if (jwtSettings == null)
            throw new ArgumentNullException(nameof(jwtSettings));

        services.AddSingleton(jwtSettings);
        services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.ValidIssuer,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.ValidAudience,
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                };
            });
    }

    private static void AddMasstransitConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetSection(nameof(EventBusSettings))
            .Get<EventBusSettings>();

        if (settings == null)
            throw new ArgumentNullException(nameof(settings));
        services.AddSingleton(settings);

        var mqConnection = new Uri(settings.HostAddress);
        services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
        services.AddMassTransit(config =>
        {
            config.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(mqConnection);

                cfg.Message<SendMailForgotPasswordEvent>(x 
                    => { x.SetEntityName("send-email-forgot-password-event"); });
                cfg.Publish<SendMailForgotPasswordEvent>(x 
                    => { x.ExchangeType = ExchangeType.Fanout; });

                cfg.Message<UserUploadVideoEvent>(x 
                    => { x.SetEntityName("user-upload-video-event"); });
                cfg.Publish<UserUploadVideoEvent>(x 
                    => { x.ExchangeType = ExchangeType.Fanout; });
            });
        });
    }

    private static void AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var redisSettings = configuration.GetSection(nameof(RedisSettings))
            .Get<RedisSettings>();
        if (redisSettings == null && string.IsNullOrEmpty(redisSettings?.ConnectionString))
            throw new ArgumentNullException(nameof(redisSettings));

        services.AddSingleton<IConnectionMultiplexer>(x =>
            ConnectionMultiplexer.Connect(redisSettings.ConnectionString));
    }

    private static void AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseSettings = configuration.GetSection(nameof(MongoDbDatabaseSettings))
            .Get<MongoDbDatabaseSettings>();
        if (databaseSettings == null)
            throw new ArgumentNullException(nameof(databaseSettings));
        services.AddSingleton(databaseSettings);
        
        services.AddSingleton<IMongoClient>(x =>
            {
                var connectionString = $"{databaseSettings.ConnectionString}/{databaseSettings.DatabaseName}?authSource=admin";
                var mongoConnectionUrl = new MongoUrl(connectionString);
                var mongoClientSettings = MongoClientSettings.FromUrl(mongoConnectionUrl);
                return new MongoClient(mongoClientSettings);
            }
            );
        services.AddScoped(x => x.GetRequiredService<IMongoClient>().StartSession());
    }
}