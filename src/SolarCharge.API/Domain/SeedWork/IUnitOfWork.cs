namespace SolarCharge.API.Domain.SeedWork;

public interface IUnitOfWork
{
    Task<bool> SaveEntitiesAsync(CancellationToken stoppingToken = default);
}