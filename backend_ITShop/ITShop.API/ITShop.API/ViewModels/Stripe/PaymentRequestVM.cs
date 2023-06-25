namespace ITShop.API.ViewModels.Stripe
{
    public class PaymentRequestVM
    {
        public string? PaymentMethodId { get; set; }
        public int Amount { get; set; }
    }
}
