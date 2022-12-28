using ITShop.API.Entities;
using ITShop.API.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITShop.API.ViewModels.User
{

    public class ProductSnimiVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int DiscountID { get; set; }
        public int CategoryID { get; set; }
        public int InventoryID { get; set; }
    }
}
