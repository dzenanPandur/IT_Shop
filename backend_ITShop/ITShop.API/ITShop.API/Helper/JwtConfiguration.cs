namespace ITShop.API.Helper
{
    public class JwtConfiguration
    {
        public string Issuer { get; set; }
        public string Secret { get; set; }
        public int ExpirationAccessTokenInMinutes { get; set; }
        public int ExpirationRefreshTokenInMinutes { get; set; }
    }
}
