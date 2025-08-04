using SolarCharge.API.Application.Models;

namespace SolarCharge.API.Application.Services.Vehicles;

public interface IVehicleService
{
    Task<Vehicle?> GetVehicleAsync();
}