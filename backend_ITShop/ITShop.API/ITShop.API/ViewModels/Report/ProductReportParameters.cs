namespace ITShop.API.ViewModels.Report
{
    public class ProductReportParameters
    {
        public string? Category { get; set; }
        public string? Producer { get; set; }
        public float? MinPrice { get; set; }
        public float? MaxPrice { get; set; }
    }
}
