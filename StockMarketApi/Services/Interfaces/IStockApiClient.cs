using StockMarketApi.Models;

namespace StockMarketApi.Services.Interfaces;

public interface IStockApiClient
{
    Task<List<TickerInfo?>> GetTickers();
}