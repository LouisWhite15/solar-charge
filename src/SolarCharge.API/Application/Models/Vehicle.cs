namespace SolarCharge.API.Application.Models;

public sealed record Vehicle(long Id)
{
    public ChargeState ChargeState { get; set; }

    public Vehicle(long id, ChargeState chargeState)
        : this(id)
    {
        ChargeState = chargeState;
    }
}
