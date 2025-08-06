using SolarCharge.API.Domain.DomainEvents;

namespace SolarCharge.API.Domain.SeedWork;

public abstract record Entity
{
    public List<IDomainEvent> DomainEvents = [];

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        DomainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        DomainEvents.Clear();
    }
}