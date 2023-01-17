using ITShop.API.ViewModels.Location;
using ITShop.API.ViewModels.User;

namespace ITShop.API.Interface
{
    public interface ILocationService
    {
        Task<Message> GetAllPaged(int items_per_page = 10, int page_number = 1);

        Task<Message> Get(int id);

        Task<Message> Snimi(LocationSnimiVM x);

        Task<Message> Delete(int id);
    }
}
