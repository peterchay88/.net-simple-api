namespace StockMarketApi.Models;


public record TickerInfo(
    string TickerName,
    string TickerSymbol,
    string? Description
    );