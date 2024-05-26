using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.API.Data.Models;

public class Prescription
{
    [Key]
    [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
    public int IdPrescription { get; set; }
    [Required]
    public DateTime Date { get; set; }
    [Required]
    public DateTime DueDate { get; set; }

    [ForeignKey("Patient")]
    public int IdPatient { get; set; }
    [ForeignKey("Doctor")]
    public int IdDoctor { get; set; }

    public virtual ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
    public virtual Patient Patient { get; set; }
    public virtual Doctor Doctor { get; set; }
}