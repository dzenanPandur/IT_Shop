using ITShop.API.ViewModels.CartItems;
using ITShop.API.ViewModels.OrderDetails;

namespace ITShop.API.Interface
{
    public interface IOrderDetailsService
    {
        Task<Message> GetAllPaged(int items_per_page = 10, int page_number = 1);

        Task<Message> Get(int id);

        Task<Message> Snimi(OrderDetailsSnimiVM x);

        Task<Message> Delete(int id);
    }
}
