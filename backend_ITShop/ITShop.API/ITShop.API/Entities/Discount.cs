using System.ComponentModel.DataAnnotations;

namespace ITShop.API.Entities
{
    public class Discount
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DiscountPercent { get; set; }
    }
}
