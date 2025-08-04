using SolarCharge.API.Application.Models;

namespace SolarCharge.API.Application.Ports;

public interface ITesla
{
    Task<long?> GetVehicleIdAsync();
    Task<ChargeState?> GetChargeStateAsync(long vehicleId);
    Task StartChargingAsync();
    Task StopChargingAsync();
}