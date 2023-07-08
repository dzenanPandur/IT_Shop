using System.ComponentModel.DataAnnotations.Schema;

namespace ITShop.API.ViewModels.Order
{
    public class OrderItemVM
    {
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
        public int ProductID { get; set; }
        public int? OrderID { get; set; }

    }
}
