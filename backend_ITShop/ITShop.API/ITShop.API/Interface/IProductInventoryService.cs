using ITShop.API.ViewModels.ProductCategory;
using ITShop.API.ViewModels.ProductInventory;

namespace ITShop.API.Interface
{
    public interface IProductInventoryService
    {
        Task<Message> GetAllPaged(int items_per_page = 10, int page_number = 1);

        Task<Message> Get(int id);

        Task<Message> Snimi(ProductInventorySnimiVM x);

        Task<Message> Delete(int id);
    }
}
