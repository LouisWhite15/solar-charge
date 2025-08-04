namespace SolarCharge.API.Application.Services;

public interface IDateTimeOffsetService
{
    DateTimeOffset UtcNow { get; }
}

public class DateTimeOffsetService : IDateTimeOffsetService
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}