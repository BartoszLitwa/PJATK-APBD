namespace RCS.API.Contracts.Models.Requests;

public record CreateContractRequest(int ClientId, int SoftwareId, DateTime StartDate, DateTime EndDate, int AdditionalYearsOfSupport);