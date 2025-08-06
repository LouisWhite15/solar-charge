using SolarCharge.API.Application.Models;

namespace SolarCharge.API.Application.Services.ChargingStrategies;

public interface IChargingStrategy
{
    Task Evaluate(InverterStatusResult inverterStatusResult);
}