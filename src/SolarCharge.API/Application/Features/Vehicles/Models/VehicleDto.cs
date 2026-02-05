using SolarCharge.API.Application.Features.Vehicles.Domain;
using SolarCharge.API.Application.Features.Vehicles.Extensions;

namespace SolarCharge.API.Application.Features.Vehicles.Models;

public sealed record VehicleDto(long Id, string DisplayName, VehicleStateDto State)
{
    public VehicleDto(Vehicle vehicle)
        : this(vehicle.Id, vehicle.DisplayName, vehicle.State.ToDto())
    {
    }
}

public enum VehicleStateDto
{
    Unknown = 0,
    Offline = 1,
    Asleep = 2,
    Online = 3
}