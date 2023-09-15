using Serilog;

namespace Tiktok.API.Presentation.Extensions;

public static class ConfigureHostExtension
{
    internal static void AddConfigurationsJson(this ConfigureHostBuilder host)
    {
        host.ConfigureAppConfiguration((context, config) =>
        {
            var env = context.HostingEnvironment.EnvironmentName;
            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange:true)
                .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange:true)
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
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Application", appName)
                .Enrich.WithProperty("Environment", env)
                .ReadFrom.Configuration(context.Configuration);
        });
    }
}