namespace SolarCharge.API.Application.Shared;

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