using StockMarketApi.Models;

namespace StockMarketApi.Services.Interfaces;

public interface IStockApiClient
{
    Task<List<Ticker>> GetTickers(string active, string limit);
}