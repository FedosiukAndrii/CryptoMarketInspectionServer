using WebApp.Interfaces;

namespace WebApp.BackgroundServices;

public class MarketDataBackgroundService(IServiceProvider serviceProvider) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var marketDataUpdater = scope.ServiceProvider.GetRequiredService<IMarketDataService>();

                try
                {
                    await marketDataUpdater.EnsureDataIsUpToDateAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating BTC data: {ex.Message}");
                }

            }

            // Обчислення затримки до початку наступної години
            var now = DateTime.Now;
            var nextHour = now.AddHours(1).Date.AddHours(now.Hour + 1); // Початок наступної години
            var delay = nextHour - now;

            // Затримка до початку наступної години
            await Task.Delay(delay, stoppingToken);
        }
    }
}

