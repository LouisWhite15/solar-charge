using SolarCharge.API.Domain.DomainEvents;
using Wolverine;

namespace SolarCharge.API.Application.DomainEventHandlers;

public class VehicleCreatedEventHandler(ILogger<VehicleCreatedEventHandler> logger)
    : IWolverineHandler
{
    public Task Handle(VehicleCreatedEvent @event)
    {
        return Task.CompletedTask;
    }
}