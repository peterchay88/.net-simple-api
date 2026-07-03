namespace StockMarketApi.Models.Api;


public record Ticker(
    string TickerName,
    string TickerSymbol,
    string MarketType,
    string PrimaryExchange,
    Boolean IsActive
    );