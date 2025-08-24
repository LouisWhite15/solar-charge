using SolarCharge.API.Application.Models;

namespace SolarCharge.API.Application.Queries;

public interface IVehicleQueries
{
    Task<long?> GetVehicleIdAsync();
    Task<VehicleDto?> GetVehicleAsync();
}