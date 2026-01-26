using SolarCharge.API.Application.Features.ChatBot.Commands;
using SolarCharge.API.Application.Features.ChatBot.Domain;
using SolarCharge.API.Application.Features.Vehicles.Events;
using Wolverine;

namespace SolarCharge.API.Infrastructure.EventHandlers;

public class InferredVehicleNotChargingEventHandler(IMessageBus messageBus) : IWolverineHandler
{
    public async ValueTask HandleAsync(InferredVehicleNotChargingEvent @event, CancellationToken cancellationToken = default)
    {
        var sendChatMessageCommand = new SendChatMessageCommand(
            ChatMessageType.InferredNotCharging,
            ChatMessageTemplates.InferredNotCharging(@event.DisplayName));
        
        await messageBus.InvokeAsync(sendChatMessageCommand, cancellationToken);
    }
}