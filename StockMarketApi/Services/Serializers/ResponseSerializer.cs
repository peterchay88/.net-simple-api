using System.Text.Json;
using StockMarketApi.Data;
using StockMarketApi.Models.Api;

namespace StockMarketApi.Services.Serializers;

public class ResponseSerializer
{
    /// <summary>
    /// Maps Massive API response body to TickerInfo DTO
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    public static async Task<Dictionary<string, object>?> SerializeToDict(HttpResponseMessage response)
    {
        string responseString = await response.Content.ReadAsStringAsync();
        Dictionary<string, object>? responseDict = 
            JsonSerializer.Deserialize<Dictionary<string, object>>(responseString);

        if (responseDict == null)
        {
            throw new Exception("Upstream Response call to Massive API did not contain a response");
        }
        
        return responseDict;
    }

    public static async Task<MassiveResults?> MapToMassiveResults(HttpResponseMessage response)
    {
        var massiveResults = await response.Content.ReadFromJsonAsync<MassiveResults>();
        return massiveResults;
    }

    public static List<Ticker>? MapToTicker(MassiveResults? massiveResults)
    {
        var tickerList = new List<Ticker>();
        
        if (massiveResults?.Results == null)
        {
            return tickerList;
        }

        foreach (var tickerRow in massiveResults.Results)
        {
            var tickerJson = JsonSerializer.Serialize(tickerRow);
            var ticker = JsonSerializer.Deserialize<Ticker>(tickerJson);
           
            ticker.CreatedOn = DateTime.UtcNow;
            ticker.CreatedBy = "API USER";
            
            tickerList.Add(ticker);
        }
        
        return tickerList;
    }
}