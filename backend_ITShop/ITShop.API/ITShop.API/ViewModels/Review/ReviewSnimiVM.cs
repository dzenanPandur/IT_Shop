namespace ITShop.API.ViewModels.Review
{
    public class ReviewSnimiVM
    {
        public int Id { get; set; }
        public string ReviewText { get; set; }
        public int ReviewValue { get; set; }
        public int? ProductID { get; set; }
    }
}
