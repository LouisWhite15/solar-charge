using SolarCharge.API.Application.Extensions;
using SolarCharge.API.Domain.Entities;
using SolarCharge.API.Infrastructure.Tesla.Dtos;

namespace SolarCharge.API.Application.Models;

public sealed record VehicleDto(long Id, string DisplayName, VehicleStateDto State)
{
    public VehicleDto(Vehicle vehicle)
        : this(vehicle.Id, vehicle.DisplayName, vehicle.State.ToDto())
    {
    }

    public VehicleDto(VehicleResponse vehicleResponse)
        :this(vehicleResponse.Response.Id, vehicleResponse.Response.DisplayName, vehicleResponse.Response.State)
    {
    }
}

public enum VehicleStateDto
{
    Unknown = 0,
    Idle = 1,
    Offline = 2,
    Online = 3,
    Charging = 4,
}