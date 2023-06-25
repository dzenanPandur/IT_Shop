using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITShop.API.Entities
{
    public class Subscription
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public Guid UserID { get; set; }
        public User? User { get; set; }

        public bool isSubscribed { get; set; }
    }
}
