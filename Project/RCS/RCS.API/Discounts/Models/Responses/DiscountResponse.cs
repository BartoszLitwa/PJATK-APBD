namespace RCS.API.Discounts.Models.Responses;

public record DiscountResponse(int Id, string Name, decimal Value, DateTime StartDate, DateTime EndDate);