using InfluxDB.Client;
using Microsoft.Extensions.Options;
using SolarCharge.API.Application.Services;

namespace SolarCharge.API.Infrastructure.InfluxDB;

public class InfluxDbService(IOptions<InfluxDbOptions> influxDbOptions) 
    : IInfluxDb
{
    private readonly string _url = influxDbOptions.Value.Url;
    private readonly string _token = influxDbOptions.Value.Token;

    public void Write(Action<WriteApi> action)
    {
        using var client = new InfluxDBClient(_url, _token);
        using var write = client.GetWriteApi();
        action(write);
    }

    public async Task<T> QueryAsync<T>(Func<QueryApi, Task<T>> action)
    {
        using var client = new InfluxDBClient(_url, _token);
        var query = client.GetQueryApi();
        return await action(query);
    }
}