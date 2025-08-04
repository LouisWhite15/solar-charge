using Microsoft.EntityFrameworkCore;
using SolarCharge.API.Application.Models;
using SolarCharge.API.Application.Repositories;
using SolarCharge.API.Infrastructure.DataAccess;

namespace SolarCharge.API.Infrastructure.Tesla.Repositories;

public class VehicleRepository(
    ILogger<VehicleRepository> logger,
    ApplicationContext dbContext)
    : IVehicleRepository
{
    public async Task<Vehicle?> GetAsync()
    {
        logger.LogTrace("Retrieving Vehicle");

        return await dbContext.Vehicles.FirstOrDefaultAsync();
    }

    public async Task SetAsync(Vehicle vehicle)
    {
        logger.LogTrace("Saving Vehicle");

        await dbContext.Vehicles.AddAsync(vehicle);
        await dbContext.SaveChangesAsync();
    }
}