namespace Pharmacy.API.Patients.Models.Responses;

public class PatientInfoWithPrescriptionsAndMedicamentsResponse
{
    public IEnumerable<PrescriptionInfoWithMedicamentsResponse> Prescriptions { get; set; }
    public DateTime Birthdate { get; set; }
    public int IdPatient { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}


public class DoctorInfoResponse
{
    public int IdDoctor { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
}

public class MedicamentInfoResponse
{
    public int IdMedicament { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
    public int Dose { get; set; }
}

public class PrescriptionInfoWithMedicamentsResponse
{
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public IEnumerable<MedicamentInfoResponse> Medicaments { get; set; }
    public DoctorInfoResponse Doctor { get; set; }
}