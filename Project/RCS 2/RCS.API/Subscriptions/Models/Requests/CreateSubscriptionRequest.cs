namespace RCS.API.Softwares.Models.Requests;

public record CreateSubscriptionRequest(
    int ClientId,
    int SoftwareId,
    string Name,
    string RenewalPeriod,
    decimal Price);