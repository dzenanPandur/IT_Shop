using ITShop.API.ViewModels.Discount;
using ITShop.API.ViewModels.Order;

namespace ITShop.API.Interface
{
    public interface IOrderService
    {

        Task<Message> GetAllPaged(int items_per_page = 10, int page_number = 1);

        Task<Message> Get(int id);

        Task<Message> Create(OrderSnimiVM x);

        Task<Message> Delete(int id);
        Task<Message> GetAllForUser(int items_per_page = 10, int page_number = 1);
    }
}
