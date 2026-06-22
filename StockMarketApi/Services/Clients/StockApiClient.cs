using StockMarketApi.Models;
using StockMarketApi.Services.Interfaces;

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
    
    private Dictionary<string, string> GetTickersQueryParams()
    {
        var queryParams =  new Dictionary<string, string>()
        {
            { "apiKey", apiKey },  // API key is set in the query params
            { "market", "stocks" },
            { "active", "true" },
            { "limit", "5" }
        };
        
        return queryParams;
    }

    private Task<string> ConvertQueryParamsToString(Dictionary<string, string> queryParams)
    {
        return new FormUrlEncodedContent(queryParams).ReadAsStringAsync();
    }
    
    public async Task<TickerInfo?> GetTickers()
    {
        // Take query params and convert it into a URI friendly query string
        var queryParams = GetTickersQueryParams();
        var queryString = ConvertQueryParamsToString(queryParams);
        
        // Make API call and map out to TickerInfo DTO object
        var url  = $"{BaseUrl}?{queryString.Result}";
        var responseDict = await Client.GetFromJsonAsync<Dictionary<string, object>?>(url);

        
        var tickerInfo = await Client.GetFromJsonAsync<TickerInfo>(url);
        return tickerInfo;
    }
}