namespace StockMarketApi.Data.Entities;

public class TickersTable
{
    public int TickerKey { get; set; }
    public string TickerId { get; set; } = string.Empty;
    public string TickerName { get; set; } = string.Empty;
    public string MarketType { get; set; } = string.Empty;
    public string PrimaryExchange { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string? CreatedBy { get; set; } = string.Empty;
    public DateTime? CreatedOn { get; set; }
}
