using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.API.Data.Models;

public class PrescriptionMedicament
{
    [Key, Column(Order = 0)]
    public int IdMedicament { get; set; }
    [Key, Column(Order = 1)]
    public int IdPrescription { get; set; }

    public int Dose { get; set; }
    public string Details { get; set; }
    
    public virtual Medicament Medicament { get; set; }
    public virtual Prescription Prescription { get; set; }
}