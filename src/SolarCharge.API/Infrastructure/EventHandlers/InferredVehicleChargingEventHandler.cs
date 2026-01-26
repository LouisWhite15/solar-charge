using SolarCharge.API.Application.Features.ChatBot.Commands;
using SolarCharge.API.Application.Features.ChatBot.Domain;
using SolarCharge.API.Application.Features.Vehicles.Events;
using Wolverine;

namespace SolarCharge.API.Infrastructure.EventHandlers;

public class InferredVehicleChargingEventHandler(IMessageBus messageBus) : IWolverineHandler
{
    public async ValueTask HandleAsync(InferredVehicleChargingEvent @event, CancellationToken cancellationToken = default)
    {
        var sendChatMessageCommand = new SendChatMessageCommand(
            ChatMessageType.InferredCharging,
            ChatMessageTemplates.InferredCharging(@event.DisplayName));
        
        await messageBus.InvokeAsync(sendChatMessageCommand, cancellationToken);
    }
}