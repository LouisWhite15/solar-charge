using Coravel;
using Microsoft.Extensions.Options;
using Serilog;
using SolarCharge.API.Application;
using SolarCharge.API.Application.Invokables;
using SolarCharge.API.WebApi.Modules;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up!");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();

    builder.Services.AddSerilog((services, lc) => lc
        .ReadFrom.Configuration(builder.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console());

    builder.Services
        .AddApplication(builder.Configuration)
        .AddSqlite(builder.Configuration)
        .AddInfluxDb(builder.Configuration)
        .AddInverter(builder.Configuration);
    
    builder.Services.AddControllers();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseHttpsRedirection();

    app.MapControllers();

    app.Services.UseScheduler(s =>
    {
        var applicationOptions = app.Services.GetRequiredService<IOptions<ApplicationOptions>>();
        s.Schedule<WriteInverterStatusInvokable>()
            .Cron(applicationOptions.Value.InverterStatusCheckCron);
    });

    await app.RunAsync();
    
    Log.Information("Stopped cleanly");
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandled exception occurred during bootstrapping");
    return 1;
}
finally
{
    await Log.CloseAndFlushAsync();
}
