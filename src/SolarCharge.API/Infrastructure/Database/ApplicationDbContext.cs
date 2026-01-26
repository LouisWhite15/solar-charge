using Microsoft.EntityFrameworkCore;
using SolarCharge.API.Application.Features.Vehicles.Domain;
using SolarCharge.API.Infrastructure.Database.Entities;
using SolarCharge.API.Infrastructure.Database.EntityTypeConfigurations;
using SolarCharge.API.Infrastructure.Database.Extensions;
using Wolverine;

namespace SolarCharge.API.Infrastructure.Database;

/// <summary>
/// To create a migration against this DbContext. Run the following command from the "SolarCharge.API" directory.
/// dotnet ef migrations add MigrationNameHere -o ./Infrastructure/Database/Migrations
/// </summary>
public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    IMessageBus bus) 
    : DbContext(options)
{
    public DbSet<TeslaAuthenticationEntity> TeslaAuthentications { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new VehicleEntityTypeConfiguration().Configure(modelBuilder.Entity<Vehicle>());
        new TeslaAuthenticationEntityTypeConfiguration().Configure(modelBuilder.Entity<TeslaAuthenticationEntity>());
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken stoppingToken = default)
    {
        await bus.DispatchDomainEventsAsync(this);
        
        _ = await base.SaveChangesAsync(stoppingToken);

        return true;
    }
}