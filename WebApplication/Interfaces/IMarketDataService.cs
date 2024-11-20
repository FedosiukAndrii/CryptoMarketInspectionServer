namespace WebApp.Interfaces;

public interface IMarketDataService
{
    Task EnsureDataIsUpToDateAsync();
    Task<IEnumerable<object>> GetBtcDataAsync(DateTime? beforeTime);
}
