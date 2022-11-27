using ITShop.API.ViewModels.User;

namespace ITShop.API.Interface
{
    public interface IUserService
    {
        Task<Message> CreateUserAsMessageAsync(UserCreateVM user, CancellationToken cancellationToken);
        // Task<Message> UpdateUserAsMessageAsync(Guid Id, UserUpdateVM user, CancellationToken cancellationToken);
        Task<Message> GetUsersAsMessageAsync(CancellationToken cancellationToken);

    }
}
