using SolarCharge.API.Application.Features.ChargingStrategy.Events;
using SolarCharge.API.Application.Features.ChatBot.Commands;
using SolarCharge.API.Application.Features.ChatBot.Domain;
using Wolverine;

namespace SolarCharge.API.Infrastructure.EventHandlers;

public class ChargingStrategyDeterminedStopChargingEventHandler(
    ILogger<ChargingStrategyDeterminedStopChargingEventHandler> logger,
    IMessageBus messageBus) : IWolverineHandler
{
    public async ValueTask HandleAsync(ChargingStrategyDeterminedStopChargingEvent @event, CancellationToken cancellationToken = default)
    {
        var sendChatMessageCommand = new SendChatMessageCommand(
            ChatMessageType.StopCharging,
            ChatMessageTemplates.StopCharging(@event.WattsPulledFromGrid));
        
        await messageBus.InvokeAsync(sendChatMessageCommand, cancellationToken);
    }
}