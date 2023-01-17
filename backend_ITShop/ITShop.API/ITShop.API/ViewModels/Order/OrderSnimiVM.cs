namespace ITShop.API.ViewModels.Order
{
    public class OrderSnimiVM
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public Guid UserID { get; set; }
        public int PaymentID { get; set; }
    }
}
