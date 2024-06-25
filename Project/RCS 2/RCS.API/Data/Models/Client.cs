using System.ComponentModel.DataAnnotations;

namespace RCS.API.Data.Models;

public abstract class Client
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Address { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [Phone]
    public string PhoneNumber { get; set; }
}

public class IndividualClient : Client
{
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; }

    [Required]
    [MaxLength(11)]
    [MinLength(11)]
    public string PESEL { get; set; } // Immutable after creation
    
    public bool IsDeleted { get; set; } = false;
}

public class CompanyClient : Client
{
    [Required]
    [MaxLength(200)]
    public string CompanyName { get; set; }

    [Required]
    [MaxLength(10)]
    public string KRS { get; set; } // Immutable after creation
}