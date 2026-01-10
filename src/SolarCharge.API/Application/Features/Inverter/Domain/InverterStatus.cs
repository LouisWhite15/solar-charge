namespace SolarCharge.API.Application.Features.Inverter.Domain;

public sealed record InverterStatus(double Photovoltaic, double Grid, double Load);
