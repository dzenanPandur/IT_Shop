using ITShop.API.Interface;
using ITShop.API.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ITShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;

        public UserController(IUserService _userService)
        {
            this._userService = _userService;
        }
        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> CreateUserAsMessageAsync(UserCreateVM userCreateVM, CancellationToken cancellationToken)
        {
            var message = await _userService.CreateUserAsMessageAsync(userCreateVM, cancellationToken);
            if (message.IsValid == false)
                return BadRequest(message);
            return Ok(message);
        }
        [HttpGet, Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsersAsMessageAsync(CancellationToken cancellationToken)
        {
            var message = await _userService.GetUsersAsMessageAsync(cancellationToken);
            if (message.IsValid == false)
                return BadRequest(message);
            return Ok(message);
        }
    }
}
