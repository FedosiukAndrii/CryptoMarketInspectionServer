using Microsoft.AspNetCore.Mvc;
using WebApp.Interfaces;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MarketDataController(IMarketDataService marketDataService) : ControllerBase
{
    [HttpGet("btcdata")]
    public async Task<IActionResult> GetBtcData([FromQuery] DateTime? beforeTime)
    {
        try
        {
            var btcData = await marketDataService.GetBtcDataAsync(beforeTime);
            return Ok(btcData);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("fillMissingData")]
    public async Task<IActionResult> InitialUpload()
    {
        try
        {
            await marketDataService.EnsureDataIsUpToDateAsync();

            return Ok("Data refreshed successfuly!");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }
}
