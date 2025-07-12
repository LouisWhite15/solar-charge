using SolarCharge.API.Application.Models;

namespace SolarCharge.API.Application.Services;

public class VehicleNotChargingStrategy : IChargingStrategy
{
    public Task Evaluate(InverterStatusResult inverterStatusResult)
    {
        throw new NotImplementedException();
    }
}