using Microsoft.EntityFrameworkCore;

namespace SolarCharge.ChatBot.Infrastructure.DataAccess;

/// <summary>
/// To create a migration against this DbContext. Run the following command from the "SolarCharge.ChatBot" directory.
/// dotnet ef migrations add MigrationNameHere -o .\Infrastructure\DataAccess\Migrations
/// </summary>
public class BotContext : DbContext
{
    public DbSet<BotEntity> Bots { get; set; }
    
    public BotContext(DbContextOptions<BotContext> options) : base(options) { }
}