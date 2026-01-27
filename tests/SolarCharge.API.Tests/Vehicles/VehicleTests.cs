using Shouldly;
using SolarCharge.API.Application.Features.Vehicles.Domain;
using SolarCharge.API.Application.Features.Vehicles.Events;

namespace SolarCharge.API.Tests.Vehicles;

public class VehicleTests
{
    [Fact]
    public void UpdateState_WhenOnlineForTwentyMinutes_IsCharging()
    {
        // Arrange
        var lastUpdated = new DateTimeOffset(2026, 01, 26, 0, 0, 0, TimeSpan.Zero);
        var vehicle = new Vehicle(1, "Test", VehicleState.Online, lastUpdated)
        {
            IsCharging = false
        };
        
        // Act
        var now = lastUpdated.AddMinutes(20);
        vehicle.UpdateState(VehicleState.Online, now);
        
        // Assert
        vehicle.IsCharging.ShouldBeTrue();
        vehicle.DomainEvents
            .OfType<InferredVehicleChargingEvent>()
            .ShouldHaveSingleItem();
    }
    
    [Fact]
    public void UpdateState_WhenStateChangedAwayFromOnline_AndIsCharging_IsNotCharging()
    {
        // Arrange
        var vehicle = new Vehicle(1, "Test", VehicleState.Online, DateTimeOffset.Now)
        {
            IsCharging = true
        };
        
        // Act
        vehicle.UpdateState(VehicleState.Asleep, DateTimeOffset.Now.AddSeconds(1));
        
        // Assert
        vehicle.IsCharging.ShouldBeFalse();
        vehicle.DomainEvents
            .OfType<InferredVehicleNotChargingEvent>()
            .ShouldHaveSingleItem();
    }

    [Fact]
    public void UpdateState_WhenOnlineForTwentyMinutes_AndAlreadyCharging_ShouldNotEmitEvent()
    {
        // Arrange
        var lastUpdated = new DateTimeOffset(2026, 01, 26, 0, 0, 0, TimeSpan.Zero);
        var vehicle = new Vehicle(1, "Test", VehicleState.Online, lastUpdated)
        {
            IsCharging = true
        };
        
        // Act
        var now = lastUpdated.AddMinutes(20);
        vehicle.UpdateState(VehicleState.Online, now);
        
        // Assert
        vehicle.IsCharging.ShouldBeTrue();
        vehicle.DomainEvents
            .OfType<InferredVehicleChargingEvent>()
            .ShouldBeEmpty();
    }

    [Fact]
    public void UpdateState_WhenNotOnlineForTwentyMinutes_ShouldNotEmitEvent()
    {
        // Arrange
        var vehicle = new Vehicle(1, "Test", VehicleState.Online, DateTimeOffset.Now)
        {
            IsCharging = false
        };
        
        // Act
        vehicle.UpdateState(VehicleState.Asleep, DateTimeOffset.Now.AddSeconds(1));
        
        // Assert
        vehicle.IsCharging.ShouldBeFalse();
        vehicle.DomainEvents
            .OfType<InferredVehicleNotChargingEvent>()
            .ShouldBeEmpty();
    }
}