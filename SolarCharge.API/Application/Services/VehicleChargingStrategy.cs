using SolarCharge.API.Application.Models;

namespace SolarCharge.API.Application.Services;

public class VehicleChargingStrategy : IChargingStrategy
{
    public Task Evaluate(InverterStatusResult inverterStatusResult)
    {
        throw new NotImplementedException();
    }
}