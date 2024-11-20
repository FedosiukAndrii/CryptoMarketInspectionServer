using Binance.Net.Clients;
using WebApp.BackgroundServices;
using WebApp.Interfaces;
using WebApp.Services;

namespace WebApp.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IMarketDataService, MarketDataService>();
        services.AddSingleton<BinanceRestClient>();

        services.AddSingleton<BinanceRestClient>();
        services.AddSingleton<MarketDataStreamingService>();

        services.AddHostedService<MarketDataBackgroundService>();

        return services;
    }
}
