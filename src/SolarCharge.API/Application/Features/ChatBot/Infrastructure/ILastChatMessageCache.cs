using SolarCharge.API.Application.Features.ChatBot.Domain;

namespace SolarCharge.API.Application.Features.ChatBot.Infrastructure;

public interface ILastChatMessageCache
{
    Task<ChatMessage?> GetAsync(CancellationToken cancellationToken = default);
    Task SetAsync(ChatMessage chatMessage, CancellationToken cancellationToken = default);
}