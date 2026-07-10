using StockMarketApi.Data.Entities;
using StockMarketApi.Models.Api;

namespace StockMarketApi.Data.Repositories;

public class TickersRepository
{
    private readonly AppDbContext _db;
    
    public TickersRepository(AppDbContext db)
    {
        _db = db;
    }
    
    /**
     * Inserts a list of Ticker objects into the database.
     * @param tickers The list of Ticker objects to insert.
     * @return The number of state entries written to the database.
     */
    public async Task<int> InsertTickers(List<Ticker> tickers)
    {
        IEnumerable<TickersTable> tickersEntity = tickers.Select(t => new TickersTable
        {
            TickerId = t.TickerSymbol,
            TickerName = t.TickerName,
            MarketType = t.MarketType,
            PrimaryExchange = t.PrimaryExchange,
            IsActive = t.IsActive,
            CreatedBy = t.CreatedBy,
            CreatedOn = t.CreatedOn,
        });
        
        await _db.TickersTable.AddRangeAsync(tickersEntity);
        return await _db.SaveChangesAsync();
    }
    
}