namespace Pharmacy.API.Prescriptions.Models.Responses;

public class AddPrescriptionResponse
{
    public int IdPrescription { get; set; }
    public IEnumerable<int> IdMedicaments { get; set; }
}