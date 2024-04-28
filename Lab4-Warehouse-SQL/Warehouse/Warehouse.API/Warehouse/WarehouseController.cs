using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Warehouse.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WarehouseController(ILogger<WarehouseController> logger, ISender sender)
    : ControllerBase
{
    private readonly ILogger<WarehouseController> _logger = logger;
    private readonly ISender _sender = sender;

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IActionResult> Get()
    {
        var result = await _sender.Send();
        return Ok(result);
    }
}