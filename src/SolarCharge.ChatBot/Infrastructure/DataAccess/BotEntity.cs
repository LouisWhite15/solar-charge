using Microsoft.AspNetCore.Identity;

namespace SolarCharge.ChatBot.Infrastructure.DataAccess;

public class BotEntity
{
    public long ChatId { get; set; }

    public BotEntity()
    {
    }

    public BotEntity(long chatId)
    {
        ChatId = chatId;
    }
}
