using Microsoft.AspNetCore.Mvc;
using StockMarketApi.Models;
using StockMarketApi.Services.Clients;

namespace StockMarketApi.Controllers;

public class TickerController : ControllerBase
{
    [HttpGet]
    [Route("api/ticker")] 
    public async Task<ActionResult<List<TickerInfo>>> GetTickerInfo()
    {
        var stockApiClient = new StockApiClient(new HttpClient());
        List<TickerInfo> tickerInfo = await stockApiClient.GetTickers();

        if (tickerInfo.Count == 0)
        {
            return NotFound("No ticker information found.");
        }
        return tickerInfo;
        
    }
}
