using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITShop.API.Entities
{
    public class ProductPicture
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public int FileSize { get; set; }
        [ForeignKey(nameof(Product))]
        public int ProductID { get; set; }
        [JsonIgnore]
        public Product Product { get; set; }
    }
}
