using ITShop.API.ViewModels.Subscription;

namespace ITShop.API.Interface
{
    public interface ISubscriptionService
    {
        Task<Message> CreateSubscriptionAsMessageAsync(SubscriptionCreateVM subscriptionCreateVM, CancellationToken cancellationToken);
        //Task<Message> DeleteSubscriptionAsMessageAsync(int id, CancellationToken cancellationToken); //ne radi kako treba
        Task<Message> SubscriptionsGetAsMessageAsync(CancellationToken cancellationToken);
        Task<Message> SubscriptionsGetByIdAsMessageAsync(Guid id, CancellationToken cancellationToken);
        Task<Message> SubscriptionUpdateAsMessageAsync(SubscriptionUpdateVM subscriptionUpdateVM, CancellationToken cancellationToken);
    }
}
