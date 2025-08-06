using SolarCharge.API.Application.Models;
using SolarCharge.API.Domain;
using SolarCharge.API.Domain.Entities;

namespace SolarCharge.API.Application.Services.Vehicles;

public interface IVehicleService
{
    Task<VehicleDto?> GetVehicleAsync();
}