using System.Text.Json;
using StockMarketApi.Data;
using StockMarketApi.Data.Repositories;
using StockMarketApi.Models.Api;
using StockMarketApi.Services.Interfaces;
using StockMarketApi.Services.Serializers;

namespace StockMarketApi.Services.Clients;

public class StockApiClient: IStockApiClient
{
    
    private readonly HttpClient _client;
    private AppDbContext _dbContext;
    private TickersRepository tickersRepository;
    
    private const string BaseUrl = "https://api.massive.com/v3/reference/tickers";
    
    private static readonly string ApiKey =
        Environment.GetEnvironmentVariable("API_KEY")
        ?? throw new InvalidOperationException("API_KEY is missing");
    
    public StockApiClient(HttpClient httpClient, AppDbContext dbContext)
    {
        _dbContext = dbContext;
        tickersRepository = new TickersRepository(_dbContext);
        _client = new HttpClient();
    }
    
    /*
     * Creates a dictionary of query parameters for the API request.
     * @param active A string indicating whether to fetch active tickers ("true" or "false").
     * @param limit A string indicating the maximum number of tickers to fetch.
     * @return A dictionary containing the query parameters.
     */
    private Dictionary<string, string> GetTickersQueryParams(string active, string limit)
    {
        var queryParams =  new Dictionary<string, string>()
        {
            { "apiKey", ApiKey },  // API key is set in the query params
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
    
    public async Task<List<Ticker>> GetTickers(string active, string limit)
    {
        // Take query params and convert it into a URI friendly query string
        Dictionary<string, string> queryParams = GetTickersQueryParams(active, limit);
        Task<string> queryString = ConvertQueryParamsToString(queryParams);
        
        // Make API call 
        string url  = $"{BaseUrl}?{queryString.Result}";
        HttpResponseMessage response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        
        // Convert response object to a Dictionary
        Dictionary<string, object>? responseDict = await ResponseSerializer.SerializeToDict(response);
        
        // Create a list of Ticker Objects
        List<Ticker> tickerInfoList = new List<Ticker>();
        if (responseDict?["results"] is JsonElement resultsDict && resultsDict.ValueKind == JsonValueKind.Array)
        {
            foreach (var ticker in resultsDict.EnumerateArray())
            {
                var dict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(ticker.GetRawText());
                
                tickerInfoList.Add(new Ticker(
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
    
    /*
     * Fetches tickers from the API and inserts them into the database.
     * @param active A string indicating whether to fetch active tickers ("true" or "false").
     * @param limit A string indicating the maximum number of tickers to fetch.
     * @return The number of state entries written to the database.
     */
    public async Task<int> FetchTickers(string active, string limit)
    {
        Dictionary<string, string> queryParams = GetTickersQueryParams(active, limit);
        Task<string> queryString = ConvertQueryParamsToString(queryParams);
        
        // Make API call 
        string url  = $"{BaseUrl}?{queryString.Result}";
        HttpResponseMessage response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        
        // Convert response object to a Dictionary
        Dictionary<string, object>? responseDict = await ResponseSerializer.SerializeToDict(response);
        
        // Create a list of Ticker Objects
        List<Ticker> tickerList = new List<Ticker>();
        if (responseDict?["results"] is JsonElement resultsDict && resultsDict.ValueKind == JsonValueKind.Array)
        {
            foreach (var ticker in resultsDict.EnumerateArray())
            {
                var dict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(ticker.GetRawText());

                tickerList.Add(new Ticker(
                    dict["name"].GetString() ?? string.Empty,
                    dict["ticker"].GetString() ?? string.Empty,
                    dict["market"].GetString() ?? string.Empty,
                    dict["primary_exchange"].GetString() ?? string.Empty,
                    dict["active"].GetBoolean()
                ));
            }
        }

        int tickersRetrieved = tickerList.Count;
        int tickersInserted = tickersRepository.InsertTickers(tickerList).Result;
        
        return tickersInserted;
    }
}