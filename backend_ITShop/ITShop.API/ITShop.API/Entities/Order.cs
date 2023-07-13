using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ITShop.API.Entities
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }

        [ForeignKey(nameof(User))]
        public Guid UserID { get; set; }
        public User? User { get; set; }
        public string Payment_intent_id { get; set; }
        public string Receipt_url { get; set; }
        public bool? isSubscribed { get; set; }
        public double TotalTotalPrice { get; set; }
        public int Quantity { get; set; }
        public string ShippingAdress { get; set; }
        public ICollection<OrderItems> OrderItems { get; set; }
    }
}
