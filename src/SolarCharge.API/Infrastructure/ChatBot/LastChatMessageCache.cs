using SolarCharge.API.Application.Features.ChatBot.Domain;
using SolarCharge.API.Application.Features.ChatBot.Infrastructure;

namespace SolarCharge.API.Infrastructure.ChatBot;

public class LastChatMessageCache : ILastChatMessageCache
{
    private ChatMessage? _last;
    private readonly Lock _lock = new();
    
    public ValueTask<ChatMessage?> GetAsync(CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            return ValueTask.FromResult(_last);
        }
    }

    public ValueTask SetAsync(ChatMessage chatMessage, CancellationToken cancellationToken = default)
    {
        lock (_lock)
        {
            _last = chatMessage;
        }

        return ValueTask.CompletedTask;
    }
}