using Microsoft.AspNetCore.Mvc;
using StockMarketApi.Data;
using StockMarketApi.Models.Api;
using StockMarketApi.Models;
using StockMarketApi.Services.Clients;

namespace StockMarketApi.Controllers;

public class TickerController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly HttpClient _httpClient;
    private StockApiClient _stockApiClient;
    private readonly ILogger<StockApiClient> _logger;
    
    public TickerController(AppDbContext dbContext, ILogger<StockApiClient> logger)
    {
        _httpClient = new HttpClient();
        _dbContext = dbContext;
        _logger = logger;
        _stockApiClient = new StockApiClient(_httpClient, _dbContext, _logger);
    }
    
    [HttpGet]
    [Route("api/ticker")] 
    public async Task<ActionResult> GetTickerInfo(
        Boolean active = true,
        string limit = "5")
    {
        List<Ticker> tickerInfoList = await _stockApiClient.GetTickers(active.ToString(), limit);

        if (tickerInfoList.Count == 0)  
        {
            return NoContent();
        }
        
        JsonApi<Ticker> response = new JsonApi<Ticker>(tickerInfoList);
        return Ok(response);
        
    }

    [HttpPost]
    [Route("api/ticker")]
    public async Task<ActionResult> PostTickerInfo(
        bool active = true,
        string limit = "0")
    {
        int tickersSaved = _stockApiClient.FetchTickers(active.ToString(), limit).Result;
        return Ok(tickersSaved);
    }
}
