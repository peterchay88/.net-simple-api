using Microsoft.EntityFrameworkCore;
using StockMarketApi.Data.Entities;


namespace StockMarketApi.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<TickersTable> TickersTable => Set<TickersTable>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TickersTable>(entity =>
        {
            entity.ToTable("tickers");

            entity.HasKey(e => e.TickerKey);
            entity.Property(e => e.TickerKey).HasColumnName("ticker_key");
            entity.Property(e => e.TickerId).HasColumnName("ticker_id");
            entity.Property(e => e.TickerName).HasColumnName("ticker_name");
            entity.Property(e => e.MarketType).HasColumnName("market_type");
            entity.Property(e => e.PrimaryExchange).HasColumnName("primary_exchange");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedOn).HasColumnName("created_on");
        });
    }
}