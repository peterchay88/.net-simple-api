using StockMarketApi.Data;
using StockMarketApi.Data.Repositories;
using StockMarketApi.Models.Api;
using StockMarketApi.Services.Interfaces;
using StockMarketApi.Services.Mappers;
using StockMarketApi.Services.Serializers;

namespace StockMarketApi.Services.Clients;

public class StockApiClient: IStockApiClient
{
    
    private readonly HttpClient _client;
    private AppDbContext _dbContext;
    private readonly TickersRepository _tickersRepository;
    private const string BaseUrl = "https://api.massive.com/v3/reference/tickers";
    private readonly ILogger<StockApiClient> _logger;
    private TickerMapper _tickerMapper;
    
    private static readonly string ApiKey =
        Environment.GetEnvironmentVariable("API_KEY")
        ?? throw new InvalidOperationException("API_KEY is missing");
    
    public StockApiClient(
        HttpClient httpClient, 
        AppDbContext dbContext,  
        ILogger<StockApiClient> logger,
        TickerMapper tickerMapper)
    {
        _dbContext = dbContext;
        _tickersRepository = new TickersRepository(_dbContext);
        _client = httpClient;
        _logger = logger;
        _tickerMapper = tickerMapper;
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
        HttpResponseMessage? response = null;
        try
        {
            var url  = $"{BaseUrl}?{queryString.Result}";
            response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, 
                $"Error in upstream api call. Returned status code: {(int)response.StatusCode}");
            
            // TODO: Add logic to return JSON API ERROR
        }
        
        // From the upstream call the objects are wrapped in a results array 
        // Serialize the upstream response into Results model
        var massiveResults = await ResponseSerializer.MapToMassiveResults(response);
        
        // Serialize Results model into a list of Ticker models
        var tickerList = _tickerMapper.MapMassiveResultsToTicker(massiveResults);

        var tickersRetrieved = tickerList.Count;
        var tickersInserted = _tickersRepository.InsertTickers(tickerList).Result;
        
        return tickersInserted;
    }
}