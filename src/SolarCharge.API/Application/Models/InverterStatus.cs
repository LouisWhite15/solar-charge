namespace SolarCharge.API.Domain.ValueObjects;

public sealed record InverterStatus(double Photovoltaic, double Grid, double Load);
