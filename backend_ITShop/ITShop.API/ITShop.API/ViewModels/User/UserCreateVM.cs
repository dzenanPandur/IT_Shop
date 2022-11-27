using ITShop.API.Enums;

namespace ITShop.API.ViewModels.User
{

    public class UserCreateVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Gender Gender { get; set; }
        public List<Guid> UserRoles { get; set; }
    }
}
