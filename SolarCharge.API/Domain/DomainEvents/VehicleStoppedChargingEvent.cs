using MediatR;

namespace SolarCharge.API.Domain.DomainEvents;

public sealed record VehicleStoppedChargingEvent(Guid Id) : INotification;
