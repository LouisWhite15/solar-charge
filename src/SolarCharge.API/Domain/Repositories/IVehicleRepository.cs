using SolarCharge.API.Domain.Entities;
using SolarCharge.API.Domain.SeedWork;

namespace SolarCharge.API.Domain.Repositories;

public interface IVehicleRepository : IRepository<Vehicle>
{
    Task<Vehicle?> GetAsync();
    Task AddAsync(Vehicle vehicle);
    Task UpdateAsync(Vehicle vehicle);
    Task DeleteAsync(long id);
}