namespace SolarCharge.API.Infrastructure.InfluxDB.Extensions;

public static class TimeSpanExtensions
{
    public static string ToInfluxQueryRange(this TimeSpan timeSpan)
    {
        var isQueryInMinutes = Math.Abs(timeSpan.Minutes) > 1;
	
        var range = isQueryInMinutes ? timeSpan.Minutes : timeSpan.Seconds;
        var suffix = isQueryInMinutes ? "m" : "s";

        return $"{range}{suffix}";
    }
}