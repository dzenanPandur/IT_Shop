using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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

        [ForeignKey(nameof(PaymentDetails))]
        public int PaymentID { get; set; }
        public PaymentDetails? PaymentDetails { get; set; }
    }
}
