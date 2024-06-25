namespace RCS.API.Softwares.Models.Requests;

public record IssueSubscriptionPaymentRequest(int SubscriptionId, decimal Amount);
