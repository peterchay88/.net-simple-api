using Microsoft.AspNetCore.Mvc;
using StockMarketApi.Models;
using StockMarketApi.Services.Clients;

namespace StockMarketApi.Controllers;

public class TickerController : ControllerBase
{
    [HttpGet]
    [Route("api/ticker")] 
    public async Task<ActionResult> GetTickerInfo(
        Boolean active = true,
        string limit = "5")
    {
        var stockApiClient = new StockApiClient(new HttpClient());
        List<TickerInfo> tickerInfoList = await stockApiClient.GetTickers(active.ToString(), limit);

        if (tickerInfoList.Count == 0)
        {
            return NoContent();
        }
        
        JsonApi<TickerInfo> response = new JsonApi<TickerInfo>(tickerInfoList);
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
