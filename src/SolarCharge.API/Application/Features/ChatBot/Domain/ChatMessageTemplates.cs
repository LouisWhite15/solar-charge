namespace SolarCharge.API.Application.Features.ChatBot.Domain;

public static class ChatMessageTemplates
{
    public static string StartCharging(double wattsSuppliedToGrid)
    {
        return $"⚡ <b>Start charging!</b> Currently supplying <code>{wattsSuppliedToGrid}W</code> to the grid";
    }

    public static string StopCharging(double wattsPulledFromGrid)
    {
        return $"⚠️ <b>Stop charging!</b> Currently pulling <code>{wattsPulledFromGrid}W</code> from the grid";
    }
    
    public static string InferredCharging(string displayName)
    {
        return $"🔋 <b>{displayName}</b> is now charging (inferred)";
    }
    
    public static string InferredNotCharging(string displayName)
    {
        return $"🔌 <b>{displayName}</b> is no longer charging (inferred)";
    }
}