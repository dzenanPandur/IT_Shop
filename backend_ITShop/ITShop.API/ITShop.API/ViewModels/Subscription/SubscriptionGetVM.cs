namespace ITShop.API.ViewModels.Subscription
{
    public class SubscriptionGetVM
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public bool isSubscribed{ get; set; }
    }
}
