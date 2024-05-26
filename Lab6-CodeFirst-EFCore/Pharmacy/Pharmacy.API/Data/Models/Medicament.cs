using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.API.Data.Models;

public class Medicament
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdMedicament { get; set; }
    [Required, MaxLength(100)]
    public string Name { get; set; }
    [Required, MaxLength(100)]
    public string Description { get; set; }
    [Required, MaxLength(100)]
    public string Type { get; set; }

    public virtual ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
}