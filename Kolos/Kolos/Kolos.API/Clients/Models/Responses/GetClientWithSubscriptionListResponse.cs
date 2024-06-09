namespace Kolos.API.Clients.Models.Responses;

public class GetClientWithSubscriptionListResponse
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; }
    public int? Discount { get; set; }
    public IEnumerable<GetSubscriptionsResponse> Subscriptions { get; set; } = new List<GetSubscriptionsResponse>();
}

public class GetSubscriptionsResponse
{
    public int IdSubscription { get; set; }
    public string Name { get; set; }
    public int RenewallPeriod { get; set; }
    public int TotalPaidAmount { get; set; }
}