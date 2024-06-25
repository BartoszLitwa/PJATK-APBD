using System.ComponentModel.DataAnnotations;

namespace RCS.API.Data.Models;

public class Employee
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Login { get; set; }

    [Required]
    public string Password { get; set; } // In a real app, store hashed passwords

    [Required] 
    public bool IsAdmin { get; set; } = false;
}