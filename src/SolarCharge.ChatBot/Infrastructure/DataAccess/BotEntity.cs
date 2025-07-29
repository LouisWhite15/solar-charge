using System.ComponentModel.DataAnnotations;

namespace SolarCharge.ChatBot.Infrastructure.DataAccess;

public class BotEntity
{
    [Key]
    public Guid Id { get; set;  }
    public long ChatId { get; set; }

    public BotEntity()
    {
    }

    public BotEntity(long chatId)
    {
        Id = Guid.NewGuid();
        ChatId = chatId;
    }
}
