namespace ITShop.API.ViewModels.Order
{
    public class OrderVM
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public Guid UserID { get; set; }
        public string Payment_intent_id { get; set; }
        public string Receipt_url { get; set; }
        public double TotalTotalPrice { get; set; }
        public int Quantity { get; set; }
        public string ShippingAdress { get; set; }
        public IEnumerable<OrderItemVM> OrderItems { get; set; }
    }
}
