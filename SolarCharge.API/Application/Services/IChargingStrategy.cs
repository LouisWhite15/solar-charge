using SolarCharge.API.Application.Models;

namespace SolarCharge.API.Application.Services;

public interface IChargingStrategy
{
    Task Evaluate(InverterStatusResult inverterStatusResult);
}