using Coravel;
using Microsoft.Extensions.Options;
using Serilog;
using SolarCharge.API.Application;
using SolarCharge.API.Application.Invokables;
using SolarCharge.API.WebApi.Modules;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {SourceContext}: {Message:lj}{NewLine}{Exception}")
    .CreateBootstrapLogger();

Log.Information("Starting up!");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();

    builder.Services.AddSerilog((_, lc) => lc
        .ReadFrom.Configuration(builder.Configuration));

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

        s.Schedule<EvaluateSolarGenerationInvokable>()
            //.Cron(applicationOptions.Value.EvaluateSolarGenerationCron);
            .EverySeconds(5);
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
