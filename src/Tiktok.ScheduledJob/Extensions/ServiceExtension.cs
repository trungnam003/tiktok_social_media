
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RabbitMQ.Client;

using Tiktok.API.Application.Common.Repositories;
using Tiktok.API.Domain.Configurations;
using Tiktok.API.Domain.Repositories;
using Tiktok.API.Domain.SeedWork;
using Tiktok.API.Domain.Services;
using Tiktok.API.Infrastructure.Persistence;
using Tiktok.API.Infrastructure.Repositories;
using Tiktok.ScheduledJob.Consumers;
using Tiktok.ScheduledJob.Services;
using Tiktok.ScheduledJob.Services.Interfaces;

namespace Tiktok.ScheduledJob.Extensions;

public static class ServiceExtension
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMasstransitConfiguration(configuration);
        var emailSettings = configuration.GetSection(nameof(EmailSettings))
            .Get<EmailSettings>();
        
        var storageSettings = configuration.GetSection(nameof(DiskStorageSettings))
            .Get<DiskStorageSettings>();

        services.AddSingleton<EmailSettings>(emailSettings);
        services.AddSingleton<DiskStorageSettings>(storageSettings);
        
        services.AddEntityFramework(configuration);
        
        services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        services.AddScoped<IAudioRepository, AudioRepository>();
        services.AddScoped<IVideoRepository, VideoRepository>();

        services.AddScoped<IEmailService<MailRequest>, EmailService>();
        services.AddScoped<IEmailTemplateService, EmailTemplateService>();
        services.AddScoped<IScheduleService, ScheduledJobService>();
        services.AddScoped<IVideoUploadHandlerService, VideoUploadHandlerService>();
        
        
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
            config.AddConsumersFromNamespaceContaining<SendMailForgotPasswordConsumer>();
            config.AddConsumersFromNamespaceContaining<UserUploadVideoConsumer>();

            config.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(mqConnection);
                cfg.ReceiveEndpoint("send-mail-forgot-password-consumer", e =>
                {
                    e.ConfigureConsumeTopology = false;
                    e.Bind("send-email-forgot-password-event", b =>
                    {
                        b.Durable = true;
                        b.ExchangeType = ExchangeType.Fanout;
                    });
                    e.ConfigureConsumer<SendMailForgotPasswordConsumer>(ctx);
                });
                
                cfg.ReceiveEndpoint("user-upload-video-consumer", e =>
                {
                    e.ConfigureConsumeTopology = false;
                    e.Bind("user-upload-video-event", b =>
                    {
                        b.Durable = true;
                        b.ExchangeType = ExchangeType.Fanout;
                    });
                    e.ConfigureConsumer<UserUploadVideoConsumer>(ctx);
                });
                
            });
        });
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
            options.UseSqlServer(databaseSettings.ConnectionString);
        });
    }
}