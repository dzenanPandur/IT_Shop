namespace ITShop.API.ViewModels.Stripe
{
    public class SubscriptionRequestVM
    {
        public string PaymentMethodId { get; set; }
        public string CustomerId { get; set; }
        public string PriceId { get; set; }
    }
}
