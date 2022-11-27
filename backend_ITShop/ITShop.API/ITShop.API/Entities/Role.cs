using Microsoft.AspNetCore.Identity;

namespace ITShop.API.Entities
{
    public class Role : IdentityRole<Guid>
    {
        public string? Description { get; set; }
        public bool IsDeleted { get; set; }
    }
}
