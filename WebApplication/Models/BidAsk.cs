namespace WebApp.Models;

public class BidAskTotal
{
    public int Id { get; set; }
    public DateTime TimeStamp { get; set; }
    public int AskVolume { get; set; }
    public int BidVolume { get; set; }
}
