using ITShop.API.Enums;

namespace ITShop.API.ViewModels.User
{
    public class UserUpdateVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
    }
}
