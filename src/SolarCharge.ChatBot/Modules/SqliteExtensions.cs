using Microsoft.EntityFrameworkCore;
using SolarCharge.ChatBot.Infrastructure.DataAccess;
using ILogger = Serilog.ILogger;

namespace SolarCharge.ChatBot.Modules;

public static class SqliteExtensions
{
    public static IServiceCollection AddSqlite(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<BotContext>(
            options => options.UseSqlite(configuration.GetValue<string>("Persistence:ConnectionString")));
        
        return services;
    }

    public static void Migrate(this IServiceProvider services, ILogger logger)
    {
        logger.ForContext<Program>().Information("Migrating database");
        
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<BotContext>();
        dbContext.Database.Migrate();
        
        logger.ForContext<Program>().Information("Database migrated");
    }
}