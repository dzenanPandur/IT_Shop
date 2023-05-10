namespace ITShop.API.Helper
{
    public class Config
    {
        public static string AplikacijaURL = "https://localhost:7093/";

        public static string Slike => "product_images/";
        public static string SlikeURL => AplikacijaURL + Slike;
        public static string SlikeBazniFolder = "wwwroot/";
        public static string SlikeFolder => SlikeBazniFolder + Slike;
    }
}
