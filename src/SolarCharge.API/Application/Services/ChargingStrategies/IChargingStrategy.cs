using SolarCharge.API.Application.Features.Inverter.Domain;
using SolarCharge.API.Application.Features.Inverter.Queries;
using SolarCharge.API.Application.Features.Vehicles;
using SolarCharge.API.Application.Models;

namespace SolarCharge.API.Application.Services.ChargingStrategies;

public interface IChargingStrategy
{
    Task Evaluate(InverterTelemetryResult inverterTelemetryResult, VehicleDto vehicle);
}