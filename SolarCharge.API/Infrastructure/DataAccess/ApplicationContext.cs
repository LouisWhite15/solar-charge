using MediatR;
using Microsoft.EntityFrameworkCore;
using SolarCharge.API.Application.Services;
using SolarCharge.API.Infrastructure.Extensions;

namespace SolarCharge.API.Infrastructure.DataAccess;

public class ApplicationContext : DbContext, IUnitOfWork
{
    private readonly IMediator _mediator;
    
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

    public ApplicationContext(DbContextOptions<ApplicationContext> options, IMediator mediator) : base(options)
    {
        _mediator = mediator;
    }
    
    public new async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Dispatch Domain Events. 
        // Choices:
        // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
        // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
        // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
        // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
        await _mediator.PublishDomainEventsAsync(this);

        // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
        // performed through the DbContext will be committed
        _ = await base.SaveChangesAsync(cancellationToken);

        return true;
    }
}