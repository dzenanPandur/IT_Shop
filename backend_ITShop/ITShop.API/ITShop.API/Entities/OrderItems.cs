using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ITShop.API.Entities
{
    public class OrderItems
    {
        [Key]
        public int Id { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }

        [ForeignKey(nameof(Product))]
        public int ProductID { get; set; }
        public Product? Product { get; set; }

       
        [ForeignKey(nameof(Order))] 
        
        [JsonIgnore]
        public int? OrderID { get; set; }
       
        //public Order? Order { get; set; }

    }
}
