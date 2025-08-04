using Coravel;
using Microsoft.Extensions.Options;
using Serilog;
using SolarCharge.API.Api.Modules;
using SolarCharge.API.Application;
using SolarCharge.API.Application.Invocables;
using SolarCharge.API.Web.Components;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {SourceContext}: {Message:lj}{NewLine}{Exception}")
    .CreateBootstrapLogger();

Log.ForContext<Program>().Information("Starting up!");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();

    builder.Services.AddSerilog((_, lc) => lc
        .ReadFrom.Configuration(builder.Configuration));
    
    builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents();

    builder.Services
        .AddApplication(builder.Configuration)
        .AddSqlite(builder.Configuration)
        .AddInfluxDb(builder.Configuration)
        .AddInverter(builder.Configuration)
        .AddTesla(builder.Configuration)
        .AddChatBot(builder.Configuration);
    
    builder.Services.AddControllers();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }
    
    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
    }
    
    app.UseAntiforgery();

    app.MapControllers();

    app.Services.UseScheduler(s =>
    {
        var applicationOptions = app.Services.GetRequiredService<IOptions<ApplicationOptions>>();
        s.Schedule<WriteInverterStatusInvocable>()
            .Cron(applicationOptions.Value.InverterStatusCheckCron);

        s.Schedule<ExecuteChargingStrategyInvocable>()
            .Cron(applicationOptions.Value.EvaluateSolarGenerationCron);

        s.Schedule<RefreshTeslaAccessTokenInvocable>()
            .Cron("00 */2 * * *"); // Runs every two hours on the hour
    });
    
    app.MapStaticAssets();
    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();
    
    // Auto-migrate database on app startup
    app.Services.Migrate(Log.Logger);

    await app.RunAsync();
    
    Log.ForContext<Program>().Information("Stopped cleanly");
    return 0;
}
catch (Exception ex)
{
    Log.ForContext<Program>().Fatal(ex, "An unhandled exception occurred during bootstrapping");
    return 1;
}
finally
{
    await Log.CloseAndFlushAsync();
}
