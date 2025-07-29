using FluentValidation;
using Serilog;
using SolarCharge.ChatBot.Infrastructure.DataAccess;
using SolarCharge.ChatBot.Modules;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {SourceContext}: {Message:lj}{NewLine}{Exception}")
    .CreateBootstrapLogger();

Log.ForContext<Program>().Information("Starting up!");

try
{
    var builder = WebApplication.CreateBuilder(args);
    
    builder.Services.AddSerilog((_, lc) => lc
        .ReadFrom.Configuration(builder.Configuration));

    builder.Services
        .AddSqlite(builder.Configuration)
        .AddTelegramBot(builder.Configuration);

    builder.Services.AddControllers();
    
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    
    builder.Services.AddValidatorsFromAssemblyContaining<Program>();

    var app = builder.Build();
    
    app.UseSwagger();
    app.UseSwaggerUI();

    app.MapControllers();

    // Auto-migrate on app startup
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