using SolarCharge.API.Application.Models;

namespace SolarCharge.API.Application.Ports;

public interface ITesla
{
    Task<VehicleDto?> GetVehicleAsync();
    Task<VehicleDto?> GetVehicleStateAsync(VehicleDto vehicle);
    Task StartChargingAsync();
    Task StopChargingAsync();
}