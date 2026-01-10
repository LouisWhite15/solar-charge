using Microsoft.EntityFrameworkCore;
using SolarCharge.API.Infrastructure.DataAccess;
using Wolverine;

namespace SolarCharge.API.Application.Features.Vehicles.Queries;

public sealed record GetVehicleQuery
{
    public class Handler(ApplicationDbContext dbContext)
        : IWolverineHandler
    {
        public async ValueTask<VehicleDto?> HandleAsync(GetVehicleQuery _, CancellationToken cancellationToken)
        {
            var vehicle = await dbContext.Vehicles
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            return vehicle is not null
                ? new VehicleDto(vehicle)
                : null;
        }
    }
}