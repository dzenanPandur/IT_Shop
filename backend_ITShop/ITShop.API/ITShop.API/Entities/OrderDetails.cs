using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ITShop.API.Entities
{
    public class OrderDetails
    {
        [Key]
        public int Id { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }

        [ForeignKey(nameof(Discount))]
        public int DiscountID { get; set; }
        public Discount? Discount { get; set; }

        [ForeignKey(nameof(Order))]
        public int OrderID { get; set; }
        public Order? Order { get; set; }

        [ForeignKey(nameof(Product))]
        public int ProductID { get; set; }
        public Product? Product { get; set; }
    }
}
