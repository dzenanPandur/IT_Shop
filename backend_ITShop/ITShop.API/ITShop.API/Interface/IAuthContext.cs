using ITShop.API.ViewModels.Auth;

namespace ITShop.API.Interface
{
    public interface IAuthContext
    {
        Task<GetLoggedUserVM> GetLoggedUser();
    }
}
