using System.Runtime.CompilerServices;
using System.Text.Json;
using StockMarketApi.Models.Api;

namespace StockMarketApi.Services.Mappers;

public class TickerMapper
{
    private static readonly JsonSerializerOptions Options = new JsonSerializerOptions()
    {
        PropertyNameCaseInsensitive = true
    };
    
    private readonly ILogger<TickerMapper> _logger;
    
    public TickerMapper(ILogger<TickerMapper> logger)
    {
        _logger = logger;
    }
    
    /// <summary>
    /// Takes the Results array from the Massive API response and
    /// maps it to the Ticker Model.
    /// </summary>
    /// <param name="massiveResults"></param>
    /// <returns></returns>
    public List<Ticker> MapMassiveResultsToTicker(MassiveResults? massiveResults)
    {
        var tickerList = new List<Ticker>();
        
        if (massiveResults?.Results == null)
        {
            _logger.LogDebug("Massive Results are null. Returning empty list.");
            return tickerList;
        }
        
        _logger.LogDebug("Attempting to Map Massive Results to Ticker List. Number of results: {Count}", 
            massiveResults.Results.Count);
        foreach (var tickerRow in massiveResults.Results)
        {
            var tickerJsonString = JsonSerializer.Serialize(tickerRow);
            var ticker = JsonSerializer.Deserialize<Ticker>(tickerJsonString, Options);
           
            ticker.CreatedOn = DateTime.UtcNow;
            ticker.CreatedBy = "API USER";
            
            tickerList.Add(ticker);
        }
        
        return tickerList;
    }
}