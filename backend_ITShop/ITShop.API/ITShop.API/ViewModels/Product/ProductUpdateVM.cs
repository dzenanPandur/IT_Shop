using ITShop.API.Enums;

namespace ITShop.API.ViewModels.User
{
    public class ProductUpdateVM
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int DiscountID { get; set; }
        public int CategoryID { get; set; }
        public int InventoryID { get; set; }
    }
}
