using ITShop.API.ViewModels.Discount;

namespace ITShop.API.Interface
{
    public interface IReviewsService
    {
        Task<Message> Snimi(ViewModels.Review.ReviewSnimiVM x);

        Task<Message> Delete(int id);
    }
}
