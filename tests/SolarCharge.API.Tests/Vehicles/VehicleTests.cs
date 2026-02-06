using Shouldly;
using SolarCharge.API.Application.Features.Vehicles.Domain;
using SolarCharge.API.Application.Features.Vehicles.Events;

namespace SolarCharge.API.Tests.Vehicles;

public class VehicleTests
{
	[Fact]
	public void ApplyTelemetry_ShouldUpdateState_WhenTelemetryIsNewer()
	{
		// Arrange
		var initialTimestamp = DateTimeOffset.UtcNow;
		var vehicle = CreateVehicle(VehicleState.Offline, false, initialTimestamp);
		var telemetry = CreateVehicleTelemetry(VehicleState.Online, false, initialTimestamp.AddMinutes(1));

		// Act
		vehicle.ApplyTelemetry(telemetry);

		// Assert
		vehicle.State.ShouldBe(VehicleState.Online);
		vehicle.IsCharging.ShouldBeFalse();
		vehicle.LastUpdated.ShouldBe(telemetry.Timestamp);
	}

	[Fact]
	public void ApplyTelemetry_ShouldNotUpdateState_WhenTelemetryIsOlder()
	{
		// Arrange
		var initialTimestamp = DateTimeOffset.UtcNow;
		var vehicle = CreateVehicle(VehicleState.Offline, false, initialTimestamp);
		var telemetry = CreateVehicleTelemetry(VehicleState.Online, false, initialTimestamp.AddMinutes(-1));

		// Act
		vehicle.ApplyTelemetry(telemetry);

		// Assert
		vehicle.State.ShouldBe(VehicleState.Offline);
		vehicle.LastUpdated.ShouldBe(initialTimestamp);
	}

	[Fact]
	public void ApplyTelemetry_ShouldNotUpdateState_WhenTelemetryStateIsUnknown()
	{
		// Arrange
		var initialTimestamp = DateTimeOffset.UtcNow;
		var vehicle = CreateVehicle(VehicleState.Offline, false, initialTimestamp);
		var telemetry = CreateVehicleTelemetry(VehicleState.Unknown, false, initialTimestamp.AddMinutes(1));

		// Act
		vehicle.ApplyTelemetry(telemetry);

		// Assert
		vehicle.State.ShouldBe(VehicleState.Offline);
		vehicle.LastUpdated.ShouldBe(initialTimestamp);
	}
	
	[Fact]
	public void ApplyTelemetry_ShouldNotUpdateToCharging_WhenTelemetryIsOlder()
	{
		// Arrange
		var initialTimestamp = DateTimeOffset.UtcNow;
		var vehicle = CreateVehicle(VehicleState.Online, false, initialTimestamp);
		var telemetry = CreateVehicleTelemetry(VehicleState.Online, true, initialTimestamp.AddMinutes(-1));

		// Act
		vehicle.ApplyTelemetry(telemetry);

		// Assert
		vehicle.IsCharging.ShouldBeFalse();
		vehicle.LastUpdated.ShouldBe(initialTimestamp);
		vehicle.DomainEvents.ShouldBeEmpty();
	}
	
	[Fact]
	public void ApplyTelemetry_ShouldUpdateToCharging_WhenTelemetryIsNewer()
	{
		// Arrange
		var initialTimestamp = DateTimeOffset.UtcNow;
		var vehicle = CreateVehicle(VehicleState.Online, false, initialTimestamp);
		var telemetry = CreateVehicleTelemetry(VehicleState.Online, true, initialTimestamp.AddMinutes(1));

		// Act
		vehicle.ApplyTelemetry(telemetry);

		// Assert
		vehicle.IsCharging.ShouldBeTrue();
		vehicle.LastUpdated.ShouldBe(telemetry.Timestamp);
		vehicle.DomainEvents.ShouldHaveSingleItem().ShouldBeOfType<VehicleChargingEvent>();
	}
	
	[Fact]
	public void ApplyTelemetry_ShouldUpdateToNotCharging_WhenTelemetryIsNewer()
	{
		// Arrange
		var initialTimestamp = DateTimeOffset.UtcNow;
		var vehicle = CreateVehicle(VehicleState.Online, true, initialTimestamp);
		var telemetry = CreateVehicleTelemetry(VehicleState.Online, false, initialTimestamp.AddMinutes(1));

		// Act
		vehicle.ApplyTelemetry(telemetry);

		// Assert
		vehicle.IsCharging.ShouldBeFalse();
		vehicle.LastUpdated.ShouldBe(telemetry.Timestamp);
		vehicle.DomainEvents.ShouldHaveSingleItem().ShouldBeOfType<VehicleNotChargingEvent>();
	}

	[Fact]
	public void ApplyTelemetry_ShouldNotUpdateChargingState_WhenTelemetryChargingStateIsSame()
	{
		// Arrange
		var initialTimestamp = DateTimeOffset.UtcNow;
		var vehicle = CreateVehicle(VehicleState.Online, true, initialTimestamp);
		var telemetry = CreateVehicleTelemetry(VehicleState.Online, true, initialTimestamp.AddMinutes(1));

		// Act
		vehicle.ApplyTelemetry(telemetry);

		// Assert
		vehicle.IsCharging.ShouldBeTrue();
		vehicle.LastUpdated.ShouldBe(initialTimestamp);
		vehicle.DomainEvents.ShouldBeEmpty();
	}
	
	[Fact]
	public void ApplyTelemetry_ShouldUpdateAllStates_WhenTelemetryIsNewer()
	{
		// Arrange
		var initialTimestamp = DateTimeOffset.UtcNow;
		var vehicle = CreateVehicle(VehicleState.Offline, false, initialTimestamp);
		var telemetry = CreateVehicleTelemetry(VehicleState.Online, true, initialTimestamp.AddMinutes(1));

		// Act
		vehicle.ApplyTelemetry(telemetry);

		// Assert
		vehicle.State.ShouldBe(VehicleState.Online);
		vehicle.IsCharging.ShouldBeTrue();
		vehicle.DomainEvents.ShouldHaveSingleItem().ShouldBeOfType<VehicleChargingEvent>();
		vehicle.LastUpdated.ShouldBe(telemetry.Timestamp);
	}
	
	private static Vehicle CreateVehicle(VehicleState state, bool isCharging, DateTimeOffset lastUpdated)
	{
		return new Vehicle(
			1, 
			"Tester-rossa",
			state,
			isCharging,
			lastUpdated);
	}
	
	private static VehicleTelemetry CreateVehicleTelemetry(VehicleState state, bool isCharging, DateTimeOffset timestamp)
	{
		return new VehicleTelemetry(
			state,
			isCharging,
			timestamp);
	}
}