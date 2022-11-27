using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITShop.API.ViewModels.Role
{
    public class UserRoleVM
    {
        public Guid UserId { get; set; }
        public List<Guid> RoleIds { get; set; }
    }
}
