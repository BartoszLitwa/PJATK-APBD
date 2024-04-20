using System.ComponentModel.DataAnnotations;

namespace Animals.API.Animals.Models;

public class Animal
{
    public int Id { get; set; }
    [MaxLength(200)]
    public string Name { get; set; }
    [MaxLength(200)]
    public string? Description { get; set; }
    [MaxLength(200)]
    public string Category { get; set; }
    [MaxLength(200)]
    public string Area { get; set; }
}