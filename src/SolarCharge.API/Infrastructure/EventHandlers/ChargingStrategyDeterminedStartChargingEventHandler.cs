using SolarCharge.API.Application.Features.ChargingStrategy.Events;
using SolarCharge.API.Application.Features.ChatBot.Commands;
using SolarCharge.API.Application.Features.ChatBot.Domain;
using Wolverine;

namespace SolarCharge.API.Infrastructure.EventHandlers;

public class ChargingStrategyDeterminedStartChargingEventHandler(IMessageBus messageBus) : IWolverineHandler
{
    public async ValueTask HandleAsync(ChargingStrategyDeterminedStartChargingEvent @event, CancellationToken cancellationToken = default)
    {
        var sendChatMessageCommand = new SendChatMessageCommand(
            ChatMessageType.StartCharging,
            ChatMessageTemplates.StartCharging(@event.WattsSuppliedToGrid));
        
        await messageBus.InvokeAsync(sendChatMessageCommand, cancellationToken);
    }
}