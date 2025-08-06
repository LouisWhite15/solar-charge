using SolarCharge.API.Application.Models;
using SolarCharge.API.Application.Services.ChargingStrategies;

namespace SolarCharge.API.Application.Services.Vehicles.ChargingStrategies;

public class VehicleNotChargingStrategy : IChargingStrategy
{
    public Task Evaluate(InverterStatusResult inverterStatusResult)
    {
        throw new NotImplementedException();
    }
}