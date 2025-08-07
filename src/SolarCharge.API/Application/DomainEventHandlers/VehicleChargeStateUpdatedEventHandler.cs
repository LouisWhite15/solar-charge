using SolarCharge.API.Domain.DomainEvents;
using Wolverine;

namespace SolarCharge.API.Application.DomainEventHandlers;

public class VehicleChargeStateUpdatedEventHandler(ILogger<VehicleChargeStateUpdatedEventHandler> logger)
    : IWolverineHandler
{
    public Task Handle(VehicleChargeStateUpdatedEvent @event)
    {
        return Task.CompletedTask;
    }
}