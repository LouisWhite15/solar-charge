using SolarCharge.API.Application.Features.TeslaAuth.Domain;
using SolarCharge.API.Application.Features.TeslaAuth.Infrastructure;
using Wolverine;

namespace SolarCharge.API.Application.Features.TeslaAuth.Queries;

public sealed record GetTeslaAuthenticationTokenQuery
{
    public class Handler(
        ILogger<GetTeslaAuthenticationTokenQuery> logger,
        ITeslaAuthenticationRepository repository) : IWolverineHandler
    {
        public async ValueTask<TeslaAuthentication?> HandleAsync(GetTeslaAuthenticationTokenQuery _, CancellationToken cancellationToken = default)
        {
            logger.LogTrace("Querying Tesla Authentication Tokens");
            
            return await repository.GetAsync(cancellationToken);
        }
    }
}