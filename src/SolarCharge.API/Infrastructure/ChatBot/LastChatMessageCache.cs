using SolarCharge.API.Application.Features.ChatBot.Domain;
using SolarCharge.API.Application.Features.ChatBot.Infrastructure;

namespace SolarCharge.API.Infrastructure.ChatBot;

public class LastChatMessageCache : ILastChatMessageCache
{
    private ChatMessage? _last;
    private readonly Lock _lock = new();
    
    public Task<ChatMessage?> GetAsync(CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            return Task.FromResult(_last);
        }
    }

    public Task SetAsync(ChatMessage chatMessage, CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            _last = chatMessage;
        }

        return Task.CompletedTask;
    }
}