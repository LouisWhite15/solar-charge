using Microsoft.EntityFrameworkCore;
using SolarCharge.API.Infrastructure.DataAccess;
using ILogger = Serilog.ILogger;

namespace SolarCharge.API.Api.Modules;

public static class SqliteExtensions
{
    public static IServiceCollection AddSqlite(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(
            options => options.UseSqlite(configuration.GetValue<string>("Persistence:ConnectionString")),
                
                // This is weirdly important! Using Singleton scoping
                // of the options allows Wolverine to significantly
                // optimize the runtime pipeline of the handlers that
                // use this DbContext type
                optionsLifetime: ServiceLifetime.Singleton);
        
        return services;
    }
    
    public static void Migrate(this IServiceProvider services, ILogger logger)
    {
        logger.ForContext<Program>().Information("Migrating database");
        
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();
        
        logger.ForContext<Program>().Information("Database migrated");
    }
}