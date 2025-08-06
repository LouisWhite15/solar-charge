using MediatR;

namespace SolarCharge.API.Application.Commands;

public sealed record CreateVehicleCommand() : IRequest;
