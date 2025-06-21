using InfluxDB.Client;
using SolarCharge.API.Application.Services;

namespace SolarCharge.API.Infrastructure.InfluxDB;

public class NoOpInfluxDbService(ILogger<NoOpInfluxDbService> logger) : IInfluxDb
{
    public void Write(Action<WriteApi> action)
    {
        logger.LogDebug("NoOpInfluxDbService is registered. Writing to InfluxDB is disabled");
    }

    public Task<T> QueryAsync<T>(Func<QueryApi, Task<T>> action)
    {
        logger.LogDebug("NoOpInfluxDbService is registered. Querying from InfluxDB is disabled");
        return null;
    }
}