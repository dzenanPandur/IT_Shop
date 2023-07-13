using ITShop.API.Entities;

namespace ITShop.API.ViewModels.Order
{
    public class OrderSnimiVM
    {
        public int Id { get; set; }
        public string Payment_intent_id { get; set; }
        public string Receipt_url { get; set; }
        public bool? isSubscribed { get; set; }
        public double TotalTotalPrice { get; set; }
        public int Quantity { get; set; }
        public string ShippingAdress { get; set; }
       
        public IEnumerable<OrderItemVM> OrderItems { get; set; }

    }
}
