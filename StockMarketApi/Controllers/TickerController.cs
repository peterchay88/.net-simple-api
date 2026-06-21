using Microsoft.AspNetCore.Mvc;
using StockMarketApi.Models;

namespace StockMarketApi.Controllers;

public class TickerController : ControllerBase
{
    [HttpGet]
    [Route("api/ticker")] 
    public TickerInfo GetTickerInfo()
    {
        return new TickerInfo(
            "Test Ticker",
            "test",
            ""
        );
    }
}
