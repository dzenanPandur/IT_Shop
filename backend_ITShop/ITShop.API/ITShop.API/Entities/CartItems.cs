using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ITShop.API.Entities
{
    public class CartItems
    {
        [Key]
        public int Id { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }

        [ForeignKey(nameof(Product))]
        public int ProductID { get; set; }
        public Product? Product { get; set; }
    }
}
