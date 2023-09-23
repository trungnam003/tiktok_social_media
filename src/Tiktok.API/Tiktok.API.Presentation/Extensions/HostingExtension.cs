using Tiktok.API.Application;
using Tiktok.API.Infrastructure;
using Tiktok.API.Presentation.Middlewares;

namespace Tiktok.API.Presentation.Extensions;

public static class HostingExtension
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddSwaggerConfigurations();

        builder.Services.AddApplicationLayerServices();
        builder.Services.AddInfrastructureLayerServices(builder.Configuration);
        builder.Services.AddMiddlewares();

        // lower router
        builder.Services.Configure<RouteOptions>(options => { options.LowercaseUrls = true; });

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseMiddleware<ErrorWrapperMiddleware>();
        app.UseAuthentication()
            .UseRouting()
            .UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/", context =>
            {
                context.Response.Redirect("/swagger");
                return Task.CompletedTask;
            });
            endpoints.MapControllers();
        });

        return app;
    }
}