using SolarCharge.API.Application.Features.ChatBot.Domain;

namespace SolarCharge.API.Application.Features.ChatBot.Infrastructure;

public interface ILastChatMessageCache
{
    ValueTask<ChatMessage?> GetAsync(CancellationToken cancellationToken = default);
    ValueTask SetAsync(ChatMessage chatMessage, CancellationToken cancellationToken = default);
}