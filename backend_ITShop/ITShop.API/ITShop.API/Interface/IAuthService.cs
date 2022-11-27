using ITShop.API.Entities;
using ITShop.API.ViewModels.Auth;

namespace ITShop.API.Interface
{
    public interface IAuthService
    {
        Task<Message> Login(LoginVM loginDto, CancellationToken cancellationToken);
        Task<Message> RefreshToken(string refreshToken, CancellationToken cancellationToken);
        Task<Message> LogoutAsync(User user);
    }
}
