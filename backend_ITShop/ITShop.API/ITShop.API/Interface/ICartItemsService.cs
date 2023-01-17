using ITShop.API.ViewModels.CartItems;
using ITShop.API.ViewModels.Discount;

namespace ITShop.API.Interface
{
    public interface ICartItemsService
    {
        Task<Message> GetAllPaged(int items_per_page = 10, int page_number = 1);

        Task<Message> Get(int id);

        Task<Message> Snimi(CartItemsSnimiVM x);

        Task<Message> Delete(int id);
    }
}
