using ITShop.API.Enums;

namespace ITShop.API.ViewModels.User
{
    public class ProductGetVM
    {
        public string? Name { get; set; }
        public double? PriceMin { get; set; }
        public double? PriceMax { get; set; }
        public int? CategoryID { get; set; }
        public string? Search { get; set; }
        public List<int>? ProducerIDs { get; set; }
    }
}
