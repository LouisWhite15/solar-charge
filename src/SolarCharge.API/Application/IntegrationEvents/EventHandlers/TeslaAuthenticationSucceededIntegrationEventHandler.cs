using MediatR;
using SolarCharge.API.Application.Commands;
using SolarCharge.API.Application.IntegrationEvents.Events;

namespace SolarCharge.API.Application.IntegrationEvents.EventHandlers;

public class TeslaAuthenticationSucceededIntegrationEventHandler(
    ILogger<TeslaAuthenticationSucceededIntegrationEventHandler> logger,
    IMediator mediator)
    : INotificationHandler<TeslaAuthenticationSucceededIntegrationEvent>
{
    public async Task Handle(TeslaAuthenticationSucceededIntegrationEvent @event, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {EventType}", nameof(TeslaAuthenticationSucceededIntegrationEvent));

        var command = new CreateVehicleCommand();
        
        logger.LogInformation("Sending {CommandType}", nameof(CreateVehicleCommand));

        await mediator.Send(command, cancellationToken);
    }
}