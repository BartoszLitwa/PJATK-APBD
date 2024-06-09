namespace Kolos.API.Clients.Models.Requests;

public class AddPaymentForSubscriptionForClientRequest
{
    public int IdClient { get; set; }
    public int IdSubscription { get; set; }
    public decimal Payment { get; set; }
}