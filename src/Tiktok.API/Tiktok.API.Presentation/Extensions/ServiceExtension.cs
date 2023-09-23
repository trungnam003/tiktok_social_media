using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Extensions;
using Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Integrations;
using Tiktok.API.Presentation.Middlewares;

namespace Tiktok.API.Presentation.Extensions;

public static class ServiceExtension
{
    public static void AddSwaggerConfigurations(this IServiceCollection services)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen((options) =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Tiktok Lite API", Version = "v1",
                Contact = new OpenApiContact()
                {
                    Email = "thtn.1611.dev@gmail.com",
                    Name = "Trung Nam",
                },
                Description = "Project API Tiktok Lite - ASP .NET Core 6.0",
            });

            options.OperationFilter<SecurityRequirementsOperationFilter>(true, "Bearer");
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description =
                    "Standard Authorization header using the Bearer scheme (JWT). Example: \"Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
        });
        services.AddJsonMultipartFormDataSupport(JsonSerializerChoice.Newtonsoft);
    }

    public static void AddMiddlewares(this IServiceCollection services)
    {
        services.AddTransient<ErrorWrapperMiddleware>();
    }
}