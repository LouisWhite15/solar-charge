using MediatR;

namespace SolarCharge.API.Domain.DomainEvents;

public sealed record VehicleStartedChargingEvent(Guid Id) : INotification;
