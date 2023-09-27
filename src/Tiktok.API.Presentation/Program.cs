using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Serilog;
using Tiktok.API.Infrastructure;
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


    // limit size body request
    builder.Services.Configure<FormOptions>(options =>
    {
        // options.ValueLengthLimit = int.MaxValue;
        options.MultipartBodyLengthLimit = 30L * 1024L * 1024L;
        // options.MemoryBufferThreshold = int.MaxValue;
    });

    builder.Services.Configure<KestrelServerOptions>(options =>
    {
        options.Limits.MaxRequestBodySize = 30L * 1024L * 1024L;
    });

    var app = builder.ConfigureServices()
        .ConfigurePipeline();
    app.MigrateDatabase();
    app.ConfigureEntityMongoBuilder();
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