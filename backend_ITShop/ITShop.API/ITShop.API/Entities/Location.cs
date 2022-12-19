using System.ComponentModel.DataAnnotations;

namespace ITShop.API.Entities
{
    public class Location
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
