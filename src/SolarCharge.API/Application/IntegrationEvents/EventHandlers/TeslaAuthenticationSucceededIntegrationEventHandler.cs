using SolarCharge.API.Application.Commands;
using SolarCharge.API.Application.IntegrationEvents.Events;
using Wolverine;

namespace SolarCharge.API.Application.IntegrationEvents.EventHandlers;

public class TeslaAuthenticationSucceededIntegrationEventHandler(
    ILogger<TeslaAuthenticationSucceededIntegrationEventHandler> logger,
    IMessageBus bus)
    : IWolverineHandler
{
    public async Task Handle(TeslaAuthenticationSucceededIntegrationEvent _, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {EventType}", nameof(TeslaAuthenticationSucceededIntegrationEvent));

        var command = new CreateVehicleCommand();
        
        logger.LogInformation("Sending {CommandType}", nameof(CreateVehicleCommand));

        await bus.SendAsync(command);
    }
}