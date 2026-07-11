using Microsoft.AspNetCore.Mvc;
using StockMarketApi.Models.Api;
using StockMarketApi.Models;
using StockMarketApi.Services.Clients;

namespace StockMarketApi.Controllers;

public class TickerController : ControllerBase
{
    private readonly StockApiClient _stockApiClient;
    
    public TickerController(StockApiClient stockApiClient)
    {
        _stockApiClient = stockApiClient;
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
