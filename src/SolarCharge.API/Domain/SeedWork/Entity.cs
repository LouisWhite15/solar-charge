using MediatR;

namespace SolarCharge.API.Domain.SeedWork;

public class Entity
{
    public List<INotification> DomainEvents { get; } = new();
    
    public Guid Id { get; }

    public void AddDomainEvent(INotification domainEvent)
    {
        DomainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        DomainEvents.Clear();
    }
}