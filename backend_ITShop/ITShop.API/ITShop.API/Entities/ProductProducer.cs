using System.ComponentModel.DataAnnotations;

namespace ITShop.API.Entities
{
    public class ProductProducer
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
