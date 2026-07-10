namespace StockMarketApi.Models.Api;

public class MassiveResults
{
    public required List<Dictionary<string, object>> Results { get; set; }
}