using SolarCharge.API.Domain.Entities;

namespace SolarCharge.API.Domain.Repositories;

public interface IVehicleRepository
{
    Task<Vehicle?> GetAsync();
    Task AddAsync(Vehicle vehicle);
    Task UpdateAsync(Vehicle vehicle);
}