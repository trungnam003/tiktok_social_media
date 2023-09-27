using Hangfire;
using Hangfire.Annotations;
using Hangfire.Dashboard;
using Serilog;
using Tiktok.API.Domain.Configurations;

namespace Tiktok.ScheduledJob.Extensions;

public static class ConfigureHostExtension
{
    internal static void AddAppConfigurations(this ConfigureHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureAppConfiguration((context, config) =>
        {
            var env = context.HostingEnvironment;
            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
        });
    }
    
    internal static void AddSerilog(this ConfigureHostBuilder host)
    {
        host.UseSerilog((context, config) =>
        {
            var appName = context.HostingEnvironment.ApplicationName.ToLower().Replace(".", "-");
            var env = context.HostingEnvironment.EnvironmentName;

            config
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Application", appName)
                .Enrich.WithProperty("Environment", env)
                .ReadFrom.Configuration(context.Configuration);
        });
    }
    
    internal static IApplicationBuilder AddHangfireDashboard(this IApplicationBuilder app, IConfiguration configuration)
    {
        

        var hangfireSettings = configuration.GetSection(nameof(HangfireSettings)).Get<HangfireSettings>();
        var configDashboard = hangfireSettings.Dashboard;
        var hangfireRoute = hangfireSettings.Route;

        app.UseHangfireDashboard(hangfireRoute, new DashboardOptions
        {
            Authorization = new[] { new AuthorizationHangfireFilter() },
            DashboardTitle = configDashboard.DashboardTitle,
            StatsPollingInterval = configDashboard.StatsPollingInterval,
            AppPath = configDashboard.AppPath,
            IgnoreAntiforgeryToken = true,
            
        });
    
        return app;
    } 
    private class AuthorizationHangfireFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            return true;
        }
    }
}