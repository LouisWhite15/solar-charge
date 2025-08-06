using MediatR;
using Microsoft.EntityFrameworkCore;
using SolarCharge.API.Domain.Entities;
using SolarCharge.API.Domain.SeedWork;
using SolarCharge.API.Infrastructure.DataAccess.Entities;
using SolarCharge.API.Infrastructure.Extensions;

namespace SolarCharge.API.Infrastructure.DataAccess;

/// <summary>
/// To create a migration against this DbContext. Run the following command from the "SolarCharge.API" directory.
/// dotnet ef migrations add MigrationNameHere -o .\Infrastructure\DataAccess\Migrations
/// </summary>
public class ApplicationContext(
    DbContextOptions<ApplicationContext> options,
    IMediator mediator) 
    : DbContext(options), IUnitOfWork
{
    public DbSet<TeslaAuthenticationEntity> TeslaAuthentications { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Vehicle>()
            .HasKey(vehicle => vehicle.Id);
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        await mediator.DispatchDomainEventsAsync(this);
        
        _ = await base.SaveChangesAsync(cancellationToken);

        return true;
    }
}