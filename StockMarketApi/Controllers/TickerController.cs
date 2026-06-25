using Microsoft.AspNetCore.Mvc;
using StockMarketApi.Models;
using StockMarketApi.Services.Clients;

namespace StockMarketApi.Controllers;

public class TickerController : ControllerBase
{
    [HttpGet]
    [Route("api/ticker")] 
    public async Task<ActionResult<List<TickerInfo>>> GetTickerInfo(
        Boolean? active = true,
        string? limit = "5")
    {
        var stockApiClient = new StockApiClient(new HttpClient());
        List<TickerInfo?> tickerInfo = await stockApiClient.GetTickers(active.ToString(), limit);

        if (tickerInfo.Count == 0)
        {
            return NoContent();
        }
        return tickerInfo;
        
    }
}
