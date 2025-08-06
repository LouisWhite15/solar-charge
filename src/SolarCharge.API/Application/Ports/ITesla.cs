using SolarCharge.API.Application.Models;

namespace SolarCharge.API.Application.Ports;

public interface ITesla
{
    Task<VehicleDto?> GetVehicleAsync();
    Task<ChargeStateDto?> GetChargeStateAsync(long vehicleId);
    Task StartChargingAsync();
    Task StopChargingAsync();
}