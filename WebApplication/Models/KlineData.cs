namespace WebApp.Models;

public class KlineData
{
    public int Id { get; set; }
    public string Symbol { get; set; }
    public string Interval { get; set; }
    public DateTime OpenTime { get; set; }
    public decimal OpenPrice { get; set; }
    public decimal HighPrice { get; set; }
    public decimal LowPrice { get; set; }
    public decimal ClosePrice { get; set; }
    public decimal Volume { get; set; }
}
