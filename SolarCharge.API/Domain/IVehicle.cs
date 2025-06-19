namespace SolarCharge.API.Domain;

public interface IVehicle
{
    Guid Id { get; }

    void StartCharging();
    void StopCharging();
}