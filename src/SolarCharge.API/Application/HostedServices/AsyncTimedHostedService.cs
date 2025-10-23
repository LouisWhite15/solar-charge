namespace SolarCharge.API.Application.HostedServices;

public abstract class AsyncTimedHostedService(
    ILogger<AsyncTimedHostedService> logger,
    int periodSeconds) 
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("{ServiceName} is running", GetType().Name);

        while (!stoppingToken.IsCancellationRequested)
        {
            // When the timer should have no due-time, then do the work once now.
            await DoWorkAsync();

            using PeriodicTimer timer = new(TimeSpan.FromSeconds(periodSeconds));
            try
            {
                while (await timer.WaitForNextTickAsync(stoppingToken))
                {
                    await DoWorkAsync();
                }
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("{ServiceName} is stopping", GetType().Name);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred in {ServiceName}", GetType().Name);
            }
        }
    }

    protected abstract Task DoWorkAsync();
}