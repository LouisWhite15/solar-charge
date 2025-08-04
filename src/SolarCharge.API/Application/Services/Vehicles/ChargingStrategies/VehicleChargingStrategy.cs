using SolarCharge.API.Application.Models;

namespace SolarCharge.API.Application.Services.Vehicles.ChargingStrategies;

public class VehicleChargingStrategy : IChargingStrategy
{
    public Task Evaluate(InverterStatusResult inverterStatusResult)
    {
        throw new NotImplementedException();
    }
}