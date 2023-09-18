using Serilog;
using Tiktok.ScheduledJob.Extensions;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up ScheduledJob");
var builder = WebApplication.CreateBuilder(args);

try
{
    builder.Host.AddAppConfigurations();
    builder.Host.AddSerilog();
    
    builder.Services.AddHangfireService(builder.Configuration);

    var app = builder.Build();

    app.AddHangfireDashboard(builder.Configuration);
    

    app.Run();
    
}catch(Exception ex)
{
    var type = ex.GetType().Name;

    if (type == "StopTheHostException")
        throw;

    Log.Fatal($"Project terminated unexpectedly {ex}");
}finally
{
    Log.Fatal($"Stopping - {builder.Environment.ApplicationName}");
    Log.CloseAndFlush();
}
