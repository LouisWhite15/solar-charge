using SolarCharge.API.Application.Features.Vehicles.Domain;
using SolarCharge.API.Application.Shared.Extensions;
using SolarCharge.API.Infrastructure.Tesla.Dtos;

namespace SolarCharge.API.Application.Features.Vehicles;

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
    Offline = 1,
    Asleep = 2,
    Online = 3
}