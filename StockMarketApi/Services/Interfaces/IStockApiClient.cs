using StockMarketApi.Models;

namespace StockMarketApi.Services.Interfaces;

public interface IStockApiClient
{
    Task<TickerInfo?> GetTickers();
}