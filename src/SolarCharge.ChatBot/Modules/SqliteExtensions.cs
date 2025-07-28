using Microsoft.EntityFrameworkCore;
using SolarCharge.ChatBot.Infrastructure.DataAccess;

namespace SolarCharge.ChatBot.Modules;

public static class SqliteExtensions
{
    public static IServiceCollection AddSqlite(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<BotContext>(
            options => options.UseSqlite(configuration.GetValue<string>("Persistence__ConnectionString")));
        
        return services;
    }
}