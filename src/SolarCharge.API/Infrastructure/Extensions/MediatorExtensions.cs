using MediatR;
using SolarCharge.API.Domain.SeedWork;
using SolarCharge.API.Infrastructure.DataAccess;

namespace SolarCharge.API.Infrastructure.Extensions;

public static class MediatorExtensions
{
    public static async Task DispatchDomainEventsAsync(this IMediator mediator, ApplicationContext dbContext)
    {
        var domainEntities = dbContext.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents.Count != 0)
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        domainEntities.ToList()
            .ForEach(entity => entity.Entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
            await mediator.Publish(domainEvent);
    }
}