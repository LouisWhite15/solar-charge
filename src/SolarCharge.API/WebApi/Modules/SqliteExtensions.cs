using Microsoft.EntityFrameworkCore;
using SolarCharge.API.Infrastructure.DataAccess;

namespace SolarCharge.API.WebApi.Modules;

public static class SqliteExtensions
{
    public static IServiceCollection AddSqlite(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationContext>(
            options => options.UseSqlite(configuration.GetValue<string>("Persistence__ConnectionString")));
        
        return services;
    }
}