using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ITShop.API.Entities
{
    public class ProductInventory
    {
        [Key]
        public int Id { get; set; }
        public int Quantity { get; set; }

        [ForeignKey(nameof(Location))]
        public int LocationID { get; set; }
        public Location? Location { get; set; }
    }
}
