namespace SolarCharge.API.Application.Models;

public sealed record InverterStatus(double Photovoltaic, double Grid, double Load);
