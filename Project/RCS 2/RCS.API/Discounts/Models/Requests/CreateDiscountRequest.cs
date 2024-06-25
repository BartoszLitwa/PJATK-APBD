namespace RCS.API.Discounts.Models.Requests;

public record CreateDiscountRequest(string Name, decimal Value, DateTime StartDate, DateTime EndDate);