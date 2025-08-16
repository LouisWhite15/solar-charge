namespace SolarCharge.API.Domain.ValueObjects;

public enum VehicleState
{
    Unknown = 0,
    Offline = 1,
    Asleep = 2,
    Online = 3,
    Charging = 4,
    Driving = 5,
    Updating = 6
}