using Microsoft.EntityFrameworkCore;
using SolarCharge.API.Domain.Entities;
using SolarCharge.API.Domain.Repositories;
using SolarCharge.API.Domain.SeedWork;

namespace SolarCharge.API.Infrastructure.DataAccess.Repositories;

public class VehicleRepository(
    ILogger<VehicleRepository> logger,
    ApplicationContext dbContext)
    : IVehicleRepository
{
    public IUnitOfWork UnitOfWork => dbContext;
    
    public async Task<Vehicle?> GetAsync()
    {
        logger.LogTrace("Retrieving Vehicle");

        return await dbContext.Vehicles.SingleOrDefaultAsync();
    }

    public async Task AddAsync(Vehicle vehicle)
    {
        logger.LogTrace("Saving Vehicle. Id: {Id}", vehicle.Id);

        await dbContext.Vehicles.AddAsync(vehicle);
    }

    public Task UpdateAsync(Vehicle vehicle)
    {
        logger.LogTrace("Updating Vehicle. Id: {Id}", vehicle.Id);
        
        dbContext.Vehicles.Update(vehicle);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(long id)
    {
        logger.LogTrace("Deleting Vehicle. Id: {Id}", id);
        
        var vehicle = await dbContext.Vehicles.FindAsync(id);
        if (vehicle is null)
            return;

        dbContext.Remove(vehicle);
    }
}