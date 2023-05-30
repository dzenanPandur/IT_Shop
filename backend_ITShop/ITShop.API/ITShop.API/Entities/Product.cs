using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ITShop.API.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }

        [ForeignKey(nameof(Discount))]
        public int DiscountID { get; set; }
        public Discount? Discount { get; set; }

        [ForeignKey(nameof(ProductCategory))]
        public int CategoryID { get; set; }
        public ProductCategory? ProductCategory { get; set; }

        [ForeignKey(nameof(ProductProducer))]
        public int? ProducerID { get; set; }
        public ProductProducer? ProductProducer { get; set; }


        [ForeignKey(nameof(ProductInventory))]
        public int InventoryID { get; set; }
        public ProductInventory? ProductInventory { get; set; }

        public ICollection<ProductPicture> ProductPictures { get; set; }
    }
}
