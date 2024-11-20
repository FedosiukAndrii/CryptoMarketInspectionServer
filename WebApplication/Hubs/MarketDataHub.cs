using Microsoft.AspNetCore.SignalR;

namespace WebApp.Hubs;

public class MarketDataHub : Hub
{
    // Метод для надсилання повідомлень клієнтам
    public async Task SendMarketData(object data)
    {
        await Clients.All.SendAsync("ReceiveMarketData", data);
    }
}
