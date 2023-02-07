using ITShop.API.Enums;
using ITShop.API.ViewModels.Role;

namespace ITShop.API.ViewModels.User
{
    public class UserGetVM
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public Gender Gender { get; set; }
        public List<UserRoleGetVM> Role { get; set; }
    }
}
