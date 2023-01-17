using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ITShop.API.Entities
{
    public class PaymentDetails
    {
        [Key]
        public int Id { get; set; }
        public int Amount { get; set; }
        public int Provider { get; set; }
        public int Status { get; set; }
    }
}
