using Serilog;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {SourceContext}: {Message:lj}{NewLine}{Exception}")
    .CreateBootstrapLogger();

Log.ForContext<Program>().Information("Starting up!");

try
{
    var builder = WebApplication.CreateBuilder(args);
    
    builder.Services.AddOpenApi();

    var app = builder.Build();
    
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseHttpsRedirection();

    app.MapControllers();

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