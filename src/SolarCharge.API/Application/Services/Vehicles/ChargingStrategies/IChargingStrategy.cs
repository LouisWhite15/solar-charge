using SolarCharge.API.Application.Models;

namespace SolarCharge.API.Application.Services.Vehicles.ChargingStrategies;

public interface IChargingStrategy
{
    Task Evaluate(InverterStatusResult inverterStatusResult);
}