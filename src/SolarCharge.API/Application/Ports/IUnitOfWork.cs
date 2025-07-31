namespace SolarCharge.API.Application.Ports;

public interface IUnitOfWork
{
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
}