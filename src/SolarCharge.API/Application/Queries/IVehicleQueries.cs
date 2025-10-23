using SolarCharge.API.Application.Models;

namespace SolarCharge.API.Application.Queries;

public interface IVehicleQueries
{
    Task<VehicleDto?> GetVehicleAsync();
}