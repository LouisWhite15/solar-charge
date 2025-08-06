using SolarCharge.API.Application.Extensions;
using SolarCharge.API.Domain.Entities;

namespace SolarCharge.API.Application.Models;

public sealed record VehicleDto(long Id, string DisplayName, ChargeStateDto ChargeState)
{
    public VehicleDto(Vehicle vehicle)
        : this(vehicle.Id, vehicle.DisplayName, vehicle.ChargeState.ToDto())
    {
    }
}

public enum ChargeStateDto
{
    Unknown = 0,
    Charging = 1,
    Stopped = 2,
    Charged = 3
}