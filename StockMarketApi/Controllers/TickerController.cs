using Microsoft.AspNetCore.Mvc;
using StockMarketApi.Data;
using StockMarketApi.Models;
using StockMarketApi.Services.Clients;

namespace StockMarketApi.Controllers;

public class TickerController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly HttpClient _httpClient;
    
    public TickerController(AppDbContext dbContext)
    {
        _httpClient = new HttpClient();
        _dbContext = dbContext;
    }
    
    [HttpGet]
    [Route("api/ticker")] 
    public async Task<ActionResult> GetTickerInfo(
        Boolean active = true,
        string limit = "5")
    {
        var stockApiClient = new StockApiClient(_httpClient, _dbContext);
        List<Ticker> tickerInfoList = await stockApiClient.GetTickers(active.ToString(), limit);

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
        bool active,
        string limit)
    {
        return NoContent();
    }
}
