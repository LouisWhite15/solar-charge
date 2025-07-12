using MediatR;
using SolarCharge.API.Domain.SeedWork;
using SolarCharge.API.Infrastructure.DataAccess;

namespace SolarCharge.API.Infrastructure.Extensions;

public static class MediatorExtensions
{
    public static async Task PublishDomainEventsAsync(this IMediator mediator, ApplicationContext context)
    {
        var domainEntities = context.ChangeTracker
            .Entries<Entity>()
            .Where(entry => entry.Entity.DomainEvents.Any());

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        domainEntities.ToList()
            .ForEach(entity => entity.Entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent);
        }
    }
}