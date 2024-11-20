using AutoMapper;
using Binance.Net.Clients;
using Binance.Net.Enums;
using Microsoft.AspNetCore.SignalR;
using WebApp.DTOs;
using WebApp.Enums;
using WebApp.Hubs;
using WebApp.Models;

public class MarketDataStreamingService(IHubContext<MarketDataHub> hubContext, IMapper mapper)
{
    private BinanceSocketClient _socketClient;

    public async Task StartStreaming()
    {
        _socketClient = new BinanceSocketClient();

        var klineResult = await _socketClient.SpotApi.ExchangeData.SubscribeToKlineUpdatesAsync(
            "BTCUSDT",
            KlineInterval.OneHour,
            async (update) =>
            {
                var newKline = new KlineData
                {
                    Symbol = "BTCUSDT",
                    Interval = nameof(Interval.OneHour),
                    OpenTime = update.Data.Data.OpenTime.ToLocalTime(),
                    OpenPrice = update.Data.Data.OpenPrice,
                    HighPrice = update.Data.Data.HighPrice,
                    LowPrice = update.Data.Data.LowPrice,
                    ClosePrice = update.Data.Data.ClosePrice
                };

                var klineDTO = mapper.Map<KlineDTO>(newKline);
                await hubContext.Clients.All.SendAsync("UpdateCandle", klineDTO);
            });

        if (!klineResult.Success)
        {
            Console.WriteLine($"Kline subscription failed: {klineResult.Error}");
        }
    }
}
