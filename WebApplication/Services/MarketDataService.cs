using AutoMapper;
using Binance.Net.Clients;
using Binance.Net.Enums;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.DTOs;
using WebApp.Enums;
using WebApp.Interfaces;
using WebApp.Models;

namespace WebApp.Services;

public class MarketDataService(AppDbContext context, BinanceRestClient binanceClient, IMapper mapper) : IMarketDataService
{
    public async Task<IEnumerable<object>> GetBtcDataAsync(DateTime? beforeTime)
    {
        var query = context.KlineData
            .Where(k => k.Symbol == "BTCUSDT" && k.Interval == nameof(Interval.OneHour));

        if (beforeTime.HasValue)
        {
            query = query.Where(k => k.OpenTime < beforeTime.Value);
        }

        var result = await query
            .OrderByDescending(k => k.OpenTime)
            .Take(500) // Завантажуємо останні 500 записів
                       //.ProjectTo<KlineDTO>(mapper.ConfigurationProvider)
            .OrderBy(k => k.OpenTime) // Сортуємо по часу у зростаючому порядку
            .ToListAsync();

        return mapper.Map<IEnumerable<KlineDTO>>(result);
    }

    public async Task EnsureDataIsUpToDateAsync()
    {
        var symbol = "BTCUSDT";
        var interval = KlineInterval.OneHour;

        // Отримуємо останній запис із бази
        var lastEntry = await context.KlineData
            .Where(k => k.Symbol == symbol && k.Interval == nameof(Interval.OneHour))
            .OrderByDescending(k => k.OpenTime)
            .FirstOrDefaultAsync();

        // Якщо записів немає, встановлюємо початок із 1 січня 2024 року
        var startTime = lastEntry == null
            ? new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            : lastEntry.OpenTime.AddHours(1).ToUniversalTime();

        var endTime = DateTime.Now.ToUniversalTime(); // Поточний час у UTC

        // Якщо дані актуальні, нічого не потрібно робити
        if (startTime >= endTime)
        {
            return;
        }

        var allKlines = new List<KlineData>();

        while (startTime < endTime)
        {
            // Запитуємо дані з Binance API
            var klinesResponse = await binanceClient.SpotApi.ExchangeData.GetKlinesAsync(
                symbol,
                interval,
                startTime,
                startTime.AddHours(1000), // 1000 записів по 1 годині
                1000
            );

            if (!klinesResponse.Success || klinesResponse.Data == null)
            {
                throw new Exception($"Failed to fetch data from Binance API: {klinesResponse.Error?.Message}");
            }

            // Формуємо нові записи
            var klines = klinesResponse.Data.Select(k => new KlineData
            {
                Symbol = symbol,
                Interval = interval.ToString(),
                OpenTime = k.OpenTime.ToLocalTime(),
                OpenPrice = k.OpenPrice,
                HighPrice = k.HighPrice,
                LowPrice = k.LowPrice,
                ClosePrice = k.ClosePrice,
                Volume = k.Volume
            }).ToList();

            allKlines.AddRange(klines);

            // Оновлюємо час початку для наступного запиту
            startTime = klinesResponse.Data.Max(k => k.OpenTime).AddHours(1);
        }

        allKlines = allKlines
            .GroupBy(k => k.OpenTime)
            .Select(g => g.First())
            .ToList();


        // Зберігаємо нові дані в базу
        if (allKlines.Any())
        {
            await context.KlineData.AddRangeAsync(allKlines);
            await context.SaveChangesAsync();
        }
    }
}
