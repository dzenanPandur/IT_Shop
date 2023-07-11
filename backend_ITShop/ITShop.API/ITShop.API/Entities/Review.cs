using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ITShop.API.Entities
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        public DateTime ReviewDate { get; set; }

        [ForeignKey(nameof(User))]
        public Guid UserID { get; set; }
        public User? User { get; set; }

        public string ReviewText { get; set; }
        public int ReviewValue { get; set; }


        [ForeignKey(nameof(Product))]
        public int? ProductID { get; set; }
    }
}
