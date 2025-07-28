using Microsoft.EntityFrameworkCore;

namespace SolarCharge.ChatBot.Infrastructure.DataAccess;

public class BotContext : DbContext
{
    public DbSet<BotEntity> Bots { get; set; }
    
    public BotContext(DbContextOptions<BotContext> options) : base(options) { }
}