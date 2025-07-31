using Microsoft.EntityFrameworkCore;
using SolarCharge.API.Infrastructure.DataAccess.Entities;

namespace SolarCharge.API.Infrastructure.DataAccess;

/// <summary>
/// To create a migration against this DbContext. Run the following command from the "SolarCharge.API" directory.
/// dotnet ef migrations add MigrationNameHere -o .\Infrastructure\DataAccess\Migrations
/// </summary>
public class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options)
{
    public DbSet<TeslaAuthenticationEntity> TeslaAuthentications { get; set; }
}