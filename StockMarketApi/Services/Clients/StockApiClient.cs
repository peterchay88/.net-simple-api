using System.Text.Json;
using StockMarketApi.Models;
using StockMarketApi.Services.Interfaces;
using StockMarketApi.Services.Serializers;

namespace StockMarketApi.Services.Clients;

public class StockApiClient(HttpClient httpClient) : IStockApiClient
{
    // Class Constructors 
    private const string BaseUrl = "https://api.massive.com/v3/reference/tickers";
    private static readonly HttpClient Client = new HttpClient();
    
    private static readonly string apiKey =
        Environment.GetEnvironmentVariable("API_KEY")
        ?? throw new InvalidOperationException("API_KEY is missing");

    // End Constructors
    
    private Dictionary<string, string> GetTickersQueryParams(string active, string limit)
    {
        var queryParams =  new Dictionary<string, string>()
        {
            { "apiKey", apiKey },  // API key is set in the query params
            { "market", "stocks" },
            { "active", active },
            { "limit", limit }
        };
        
        return queryParams;
    }

    private Task<string> ConvertQueryParamsToString(Dictionary<string, string> queryParams)
    {
        return new FormUrlEncodedContent(queryParams).ReadAsStringAsync();
    }
    
    public async Task<List<TickerInfo>> GetTickers(string active, string limit)
    {
        // Take query params and convert it into a URI friendly query string
        Dictionary<string, string> queryParams = GetTickersQueryParams(active, limit);
        Task<string> queryString = ConvertQueryParamsToString(queryParams);
        
        // Make API call 
        string url  = $"{BaseUrl}?{queryString.Result}";
        HttpResponseMessage response = await Client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        
        // Convert response object to a Dictionary
        Dictionary<string, object>? responseDict = await ResponseSerializer.SerializeToDict(response);
        
        // Create a list of TickerInfo Objects
        List<TickerInfo> tickerInfoList = new List<TickerInfo>();
        if (responseDict?["results"] is JsonElement resultsDict && resultsDict.ValueKind == JsonValueKind.Array)
        {
            foreach (var ticker in resultsDict.EnumerateArray())
            {
                var dict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(ticker.GetRawText());
                
                tickerInfoList.Add(new TickerInfo(
                    dict["name"].GetString() ?? string.Empty,
                    dict["ticker"].GetString() ?? string.Empty,
                    dict["market"].GetString() ?? string.Empty,
                    dict["primary_exchange"].GetString() ?? string.Empty,
                    dict["active"].GetBoolean()
                ));
            }
        }
        
        return tickerInfoList;
    }
}