using InfluxDB.Client;
using SolarCharge.API.Application.Services;

namespace SolarCharge.API.Infrastructure.InfluxDB;

public class NoOpInfluxDbService : IInfluxDb
{
    public void Write(Action<WriteApi> action)
    {
    }

    public Task<T> QueryAsync<T>(Func<QueryApi, Task<T>> action)
    {
        return null;
    }
}