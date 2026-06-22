using Microsoft.AspNetCore.Mvc;
using StockMarketApi.Models;
using StockMarketApi.Services.Clients;

namespace StockMarketApi.Controllers;

public class TickerController : ControllerBase
{
    [HttpGet]
    [Route("api/ticker")] 
    public async Task<ActionResult<TickerInfo>> GetTickerInfo()
    {
        var stockApiClient = new StockApiClient(new HttpClient());
        var tickerInfo = await stockApiClient.GetTickers();

        if (tickerInfo is null) return NotFound();
        return tickerInfo;
        
    }
}
