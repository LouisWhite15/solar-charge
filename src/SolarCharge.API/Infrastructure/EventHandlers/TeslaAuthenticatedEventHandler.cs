using SolarCharge.API.Application.Features.TeslaAuth.Authenticate;
using SolarCharge.API.Application.Features.Vehicles.Commands;
using Wolverine;

namespace SolarCharge.API.Infrastructure.EventHandlers;

public class TeslaAuthenticatedEventHandler(
    ILogger<TeslaAuthenticatedEventHandler> logger,
    IMessageBus bus)
    : IWolverineHandler
{
    public async ValueTask HandleAsync(TeslaAuthenticatedEvent _, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {EventType}", nameof(TeslaAuthenticatedEvent));

        var command = new RegisterVehicleFromTeslaCommand();
        
        logger.LogInformation("Sending {CommandType}", nameof(RegisterVehicleFromTeslaCommand));

        await bus.InvokeAsync(command, cancellationToken);
    }
}