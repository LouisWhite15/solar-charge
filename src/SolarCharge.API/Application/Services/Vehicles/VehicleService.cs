using SolarCharge.API.Application.Models;
using SolarCharge.API.Application.Ports;
using SolarCharge.API.Application.Repositories;

namespace SolarCharge.API.Application.Services.Vehicles;

public class VehicleService(
    ILogger<VehicleService> logger,
    IVehicleRepository vehicleRepository,
    ITesla tesla) : IVehicleService
{
    public async Task<Vehicle?> GetVehicleAsync()
    {
        logger.LogTrace("Retrieving vehicle");
        
        var vehicle = await vehicleRepository.GetAsync();
        if (vehicle is null)
        {
            logger.LogInformation("No vehicle is currently set. Retrieving vehicle information from Tesla");
            return await GetVehicleFromTeslaAsync();
        }
        
        logger.LogInformation("Retrieving latest vehicle state from Tesla");
        var chargeState = await tesla.GetChargeStateAsync(vehicle.Id);
        
        vehicle.ChargeState = chargeState ?? ChargeState.Unknown;
        await vehicleRepository.SetAsync(vehicle);

        return vehicle;
    }

    private async Task<Vehicle?> GetVehicleFromTeslaAsync()
    {
        var vehicleId = await tesla.GetVehicleIdAsync();
        if (vehicleId is null)
        {
            logger.LogWarning("Could not retrieve vehicle from Tesla");
            return null;
        }

        var chargeState = await tesla.GetChargeStateAsync(vehicleId.Value);

        var vehicle = new Vehicle(
            vehicleId.Value,
            chargeState ?? ChargeState.Unknown);
        
        await vehicleRepository.SetAsync(vehicle);
        return vehicle;
    }
}