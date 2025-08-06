using MediatR;

namespace SolarCharge.API.Application.Commands;

public sealed record SetChargeStateCommand(long VehicleId) : IRequest;
