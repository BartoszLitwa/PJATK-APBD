namespace Pharmacy.API.Prescriptions.Models.Requests;

public class AddPrescriptionRequest
{
    public AddPrescriptionPatienRequest Patient { get; set; }
    public IEnumerable<AddPrescriptionMedicamentsRequest> Medicaments { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
}

public class AddPrescriptionPatienRequest
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime Birthdate { get; set; }
}

public class AddPrescriptionMedicamentsRequest
{
    public int IdMedicament { get; set; }
    public int Dose { get; set; }
    public string Description { get; set; }
}