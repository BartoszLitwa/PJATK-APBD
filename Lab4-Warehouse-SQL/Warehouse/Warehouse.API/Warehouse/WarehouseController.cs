using MediatR;
using Microsoft.AspNetCore.Mvc;
using Warehouse.API.Controllers.Commands;
using Warehouse.API.Warehouse.Commands;

namespace Warehouse.API.Warehouse;

[ApiController]
[Route("api/[controller]")]
public class WarehouseController(ILogger<WarehouseController> logger, ISender sender)
    : ControllerBase
{
    private readonly ILogger<WarehouseController> _logger = logger;
    private readonly ISender _sender = sender;

    [HttpPost("Code", Name = "In c# code")]
    public async Task<IActionResult> AddProductToWarehouseInCode([FromBody] AddProductToWarehouseRequest request)
    {
        var result = await _sender.Send(new AddProductToWarehouseInCodeCommand
        {
            ProductId = request.IdProduct,
            WarehouseId = request.IdWarehouse,
            Amount = request.Amount,
            CreatedAt = request.CreatedAt
        });
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
    
    [HttpPost("SQL", Name = "SQLProcedure")]
    public async Task<IActionResult> AddProductToWarehouseInSQLProcedure([FromBody] AddProductToWarehouseRequest request)
    {
        var result = await _sender.Send(new AddProductToWarehouseInSQLProcedureCommand
        {
            ProductId = request.IdProduct,
            WarehouseId = request.IdWarehouse,
            Amount = request.Amount,
            CreatedAt = request.CreatedAt
        });
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}