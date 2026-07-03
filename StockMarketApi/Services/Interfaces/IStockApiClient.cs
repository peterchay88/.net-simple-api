using StockMarketApi.Models.Api;

namespace StockMarketApi.Services.Interfaces;

public interface IStockApiClient
{
    Task<List<Ticker>> GetTickers(string active, string limit);
}