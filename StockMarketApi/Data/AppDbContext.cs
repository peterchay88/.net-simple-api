using Microsoft.EntityFrameworkCore;
using StockMarketApi.Models.Database;

namespace StockMarketApi.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Tickers> Tickers => Set<Tickers>();
}