using Microsoft.EntityFrameworkCore;
using SolarCharge.API.Application.Models;
using SolarCharge.API.Infrastructure.DataAccess;

namespace SolarCharge.API.Application.Queries;

public class VehicleQueries(ApplicationContext dbContext)
    : IVehicleQueries
{
    public async Task<VehicleDto?> GetVehicleAsync()
    {
        var vehicle = await dbContext.Vehicles
            .AsNoTracking()
            .FirstOrDefaultAsync();

        return vehicle is not null
            ? new VehicleDto(vehicle)
            : null;
    }
}