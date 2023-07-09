using ITShop.API.Enums;

namespace ITShop.API.ViewModels.Auth
{
    public class SessionVM
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Token { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public long TokenExpireDate { get; set; }
        public Gender Gender { get; set; }
        public List<string> Roles { get; set; }
    }
}
