using SolarCharge.API.Application.Models;
using SolarCharge.API.Domain.Repositories;

namespace SolarCharge.API.Application.Queries;

public class VehicleQueries(IVehicleRepository vehicleRepository)
    : IVehicleQueries
{
    public async Task<long?> GetVehicleIdAsync()
    {
        var vehicle = await vehicleRepository.GetAsync();
        return vehicle?.Id;
    }
    
    public async Task<VehicleDto?> GetVehicleAsync()
    {
        var vehicle = await vehicleRepository.GetAsync();

        return vehicle is not null
            ? new VehicleDto(vehicle)
            : null;
    }
}