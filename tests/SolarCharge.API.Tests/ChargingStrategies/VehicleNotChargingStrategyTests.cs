using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SolarCharge.API.Application.Features.ChargingStrategy;
using SolarCharge.API.Application.Features.ChargingStrategy.Services;
using Wolverine;

namespace SolarCharge.API.Tests.ChargingStrategies;

public class VehicleNotChargingStrategyTests
{
    private readonly Mock<IMessageBus> _messageBusMock = new();
    private readonly VehicleNotChargingStrategy _strategy;

    public VehicleNotChargingStrategyTests()
    {
        _strategy = new VehicleNotChargingStrategy(
            Mock.Of<ILogger<VehicleNotChargingStrategy>>(),
            Options.Create(new ChargingStrategyOptions
            {
                StartChargingExcessGenerationThresholdWatts = 1000
            }),
            _messageBusMock.Object);
    }
    
    [Fact]
    public async Task EvaluateAsync_ShouldInvokeStartChargingEvent_WhenGridIsSupplyingMoreThanThreshold()
    {
        
    }
}