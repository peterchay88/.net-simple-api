namespace StockMarketApi.Models.Api;


public class Ticker
{
    [System.Text.Json.Serialization.JsonPropertyName("name")]
    public required string TickerName { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("ticker")]
    public required string TickerSymbol { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("market")]
    public required string MarketType { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("primary_exchange")]
    public required string PrimaryExchange { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("active")]
    public bool IsActive { get; set; }

    public DateTime CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

};
