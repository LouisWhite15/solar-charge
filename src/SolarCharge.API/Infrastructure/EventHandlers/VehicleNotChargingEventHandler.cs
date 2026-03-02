using SolarCharge.API.Application.Features.ChatBot.Commands;
using SolarCharge.API.Application.Features.ChatBot.Domain;
using SolarCharge.API.Application.Features.Vehicles.Events;
using Wolverine;

namespace SolarCharge.API.Infrastructure.EventHandlers;

public class VehicleNotChargingEventHandler(IMessageBus messageBus) : IWolverineHandler
{
    public async Task HandleAsync(VehicleNotChargingEvent @event, CancellationToken cancellationToken = default)
    {
        var sendChatMessageCommand = new SendChatMessageCommand(
            ChatMessageType.NotCharging,
            ChatMessageTemplates.NotCharging(@event.DisplayName));
        
        await messageBus.InvokeAsync(sendChatMessageCommand, cancellationToken);
    }
}