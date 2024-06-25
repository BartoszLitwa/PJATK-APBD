namespace RCS.API.Contracts.Models.Requests;

public record IssuePaymentRequest(int ContractId, decimal Amount);