using SolarCharge.API.Domain.ValueObjects;

namespace SolarCharge.API.Application.Interfaces;

public interface IInverter
{
    Task<InverterStatus> GetAsync();
}