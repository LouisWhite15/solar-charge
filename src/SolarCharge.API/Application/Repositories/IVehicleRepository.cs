using SolarCharge.API.Application.Models;

namespace SolarCharge.API.Application.Repositories;

public interface IVehicleRepository
{
    Task<Vehicle?> GetAsync();
    Task SetAsync(Vehicle vehicle);
}