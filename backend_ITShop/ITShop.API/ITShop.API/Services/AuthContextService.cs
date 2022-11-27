using ITShop.API.Entities;
using ITShop.API.Interface;
using ITShop.API.ViewModels.Auth;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ITShop.API.Services
{
    public class AuthContextService : IAuthContext
    {
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthContextService(UserManager<User> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<GetLoggedUserVM> GetLoggedUser()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                throw new ArgumentException("Unauthorized");
            }

            var user = await _userManager.FindByIdAsync(userId);



            return new GetLoggedUserVM
            {
                Id = Guid.Parse(userId),
                FirstName = user.FirstName,
                LastName = user.LastName,

            };

        }
    }
}
