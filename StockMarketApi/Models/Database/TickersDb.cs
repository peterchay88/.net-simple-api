namespace StockMarketApi.Models.Database;

public record Tickers(
    int TickerKey,
    string TickerId,
    string TickerName,
    string MarketType,
    string PrimaryExchange,
    bool IsActive);