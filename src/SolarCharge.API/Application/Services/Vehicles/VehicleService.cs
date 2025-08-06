using SolarCharge.API.Application.Extensions;
using SolarCharge.API.Application.Models;
using SolarCharge.API.Application.Ports;
using SolarCharge.API.Domain.Entities;
using SolarCharge.API.Domain.Repositories;

namespace SolarCharge.API.Application.Services.Vehicles;

public class VehicleService(
    ILogger<VehicleService> logger,
    IVehicleRepository vehicleRepository,
    ITesla tesla) : IVehicleService
{
    public async Task<VehicleDto?> GetVehicleAsync()
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
        
        vehicle.SetChargeState(chargeState.ToDomain());
        await vehicleRepository.UpdateAsync(vehicle);

        return new VehicleDto(vehicle);
    }

    private async Task<VehicleDto?> GetVehicleFromTeslaAsync()
    {
        var vehicleDto = await tesla.GetVehicleAsync();
        if (vehicleDto is null)
        {
            logger.LogWarning("Could not retrieve vehicle from Tesla");
            return null;
        }

        var chargeState = await tesla.GetChargeStateAsync(vehicleDto.Id);

        var vehicle = new Vehicle(
            vehicleDto.Id,
            vehicleDto.DisplayName,
            chargeState.ToDomain());
        
        await vehicleRepository.AddAsync(vehicle);
        
        return new VehicleDto(vehicle);
    }
}