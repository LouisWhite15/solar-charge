using SolarCharge.API.Application.Models;

namespace SolarCharge.API.Application.Features.Vehicles.Infrastructure;

public interface ITeslaClient
{
    Task<VehicleDto?> GetVehicleAsync(CancellationToken cancellationToken = default);
    Task<VehicleDto?> GetVehicleStateAsync(long vehicleId, CancellationToken cancellationToken = default);
}