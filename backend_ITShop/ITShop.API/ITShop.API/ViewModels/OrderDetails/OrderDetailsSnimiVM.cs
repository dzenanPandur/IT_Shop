namespace ITShop.API.ViewModels.OrderDetails
{
    public class OrderDetailsSnimiVM
    {
        public int Id { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
        public int DiscountID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
    }
}
