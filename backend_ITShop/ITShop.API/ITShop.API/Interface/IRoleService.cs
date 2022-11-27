using ITShop.API.ViewModels.Role;

namespace ITShop.API.Interface
{
    public interface IRoleService
    {
        Task<Message> CreateRoleAsMessageAsync(RoleCreateVM roleCreateVM, CancellationToken cancellationToken);
        Task<Message> DeleteRoleAsMessageAsync(Guid roleId, CancellationToken cancellationToken);
        Task<Message> AddRoleToUserAsMessageAsync(UserRoleVM userRoleVM, CancellationToken cancellationToken);
    }
}
