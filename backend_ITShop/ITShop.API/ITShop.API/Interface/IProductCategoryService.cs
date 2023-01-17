using ITShop.API.ViewModels.Location;
using ITShop.API.ViewModels.ProductCategory;

namespace ITShop.API.Interface
{
    public interface IProductCategoryService
    {
        
            Task<Message> GetAllPaged(int items_per_page = 10, int page_number = 1);

            Task<Message> Get(int id);

            Task<Message> Snimi(ProductCategorySnimiVM x);

            Task<Message> Delete(int id);
        
    }
}
