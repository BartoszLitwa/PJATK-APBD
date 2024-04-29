using System.ComponentModel.DataAnnotations;

namespace Warehouse.API.Controllers.Commands;

public class AddProductToWarehouseRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    public int IdProduct { get; set; }
    [Required]
    [Range(1, int.MaxValue)]
    public int IdWarehouse { get; set; }
    [Required]
    public int Amount { get; set; }
    [Required]
    public DateTime CreatedAt { get; set; }
}