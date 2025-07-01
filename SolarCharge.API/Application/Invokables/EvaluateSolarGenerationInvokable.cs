using Coravel.Invocable;
using SolarCharge.API.Application.Services;

namespace SolarCharge.API.Application.Invokables;

public class EvaluateSolarGenerationInvokable(
    ILogger<EvaluateSolarGenerationInvokable> logger,
    IInfluxDb influxDb)
    : IInvocable
{
    public Task Invoke()
    {
        return Task.CompletedTask;
    }
}