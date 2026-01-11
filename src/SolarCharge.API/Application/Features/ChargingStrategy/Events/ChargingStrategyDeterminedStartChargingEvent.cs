namespace SolarCharge.API.Application.Features.ChargingStrategy.Events;

public sealed record ChargingStrategyDeterminedStartChargingEvent(double WattsSuppliedToGrid);
