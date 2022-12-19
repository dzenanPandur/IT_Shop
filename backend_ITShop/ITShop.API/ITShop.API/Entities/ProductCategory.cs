using System.ComponentModel.DataAnnotations;

namespace ITShop.API.Entities
{
    public class ProductCategory
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
