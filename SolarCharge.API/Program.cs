using Coravel;
using Serilog;
using Serilog.Events;
using SolarCharge.API.Application.Jobs;
using SolarCharge.API.WebApi.Modules;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSerilog((_, lc) => lc
    .ReadFrom.Configuration(builder.Configuration));

builder.Services
    .AddSqlite(builder.Configuration)
    .AddInverter(builder.Configuration)
    .AddApplication(builder.Configuration);

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
    s.Schedule<WriteInverterStatusInvokable>()
        .EveryMinute();
});

await app.RunAsync();
