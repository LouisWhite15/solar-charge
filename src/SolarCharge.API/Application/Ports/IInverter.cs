using SolarCharge.API.Application.Models;

namespace SolarCharge.API.Application.Ports;

public interface IInverter
{
    Task<InverterStatus> GetAsync();
}