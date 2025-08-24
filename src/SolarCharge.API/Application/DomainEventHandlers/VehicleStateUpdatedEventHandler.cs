using SolarCharge.API.Domain.DomainEvents;
using Wolverine;

namespace SolarCharge.API.Application.DomainEventHandlers;

public class VehicleStateUpdatedEventHandler(ILogger<VehicleStateUpdatedEventHandler> logger)
    : IWolverineHandler
{
    public Task Handle(VehicleStateUpdatedEvent @event)
    {
        return Task.CompletedTask;
    }
}