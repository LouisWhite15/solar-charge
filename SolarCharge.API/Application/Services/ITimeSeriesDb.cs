using InfluxDB.Client;

namespace SolarCharge.API.Application.Services;

public interface IInfluxDb
{
    void Write(Action<WriteApi> action);
    Task<T> QueryAsync<T>(Func<QueryApi, Task<T>> action);
}