using Tiktok.API.Presentation.Middlewares;

namespace Tiktok.API.Presentation.Extensions;

public static class ServiceExtension
{
    public static void AddSwaggerConfigurations(this IServiceCollection services)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public static void AddMiddlewares(this IServiceCollection services)
    {
        services.AddTransient<ErrorWrapperMiddleware>();
    }
}