using SolarCharge.API.Domain.ValueObjects;

namespace SolarCharge.API.Application.Services;

public interface IInverter
{
    Task<InverterStatus> GetAsync();
}