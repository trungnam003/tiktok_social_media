using Serilog;
using Tiktok.API.Infrastructure.Persistence;
using Tiktok.API.Presentation.Extensions;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up API");

var builder = WebApplication.CreateBuilder(args);

try
{
    builder.Host.AddConfigurationsJson();
    builder.Host.AddSerilog();

    var app = builder.ConfigureServices()
        .ConfigurePipeline();
    app.MigrateDatabase();
    app.Run();
}
catch (Exception ex)
{
    var type = ex.GetType().Name;

    if (type == "StopTheHostException")
        throw;

    Log.Fatal($"Project terminated unexpectedly {ex}");
}
finally
{
    Log.Fatal($"Stopping - {builder.Environment.ApplicationName}");
    Log.CloseAndFlush();
}