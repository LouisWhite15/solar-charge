using SolarCharge.API.Application.Shared;
using Wolverine;

namespace SolarCharge.API.Infrastructure.Database.Extensions;

public static class MessageBusExtensions
{
    public static async Task DispatchDomainEventsAsync(this IMessageBus bus, ApplicationDbContext dbDbContext)
    {
        var domainEntities = dbDbContext.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents.Count != 0)
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        domainEntities.ToList()
            .ForEach(entity => entity.Entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await bus.InvokeAsync(domainEvent);
        }
    }
}