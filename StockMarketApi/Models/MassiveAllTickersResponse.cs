namespace StockMarketApi.Models;

public record MassiveAllTickersResponse(
    Array Results,
    string Status,
    string RequestId
    );
    