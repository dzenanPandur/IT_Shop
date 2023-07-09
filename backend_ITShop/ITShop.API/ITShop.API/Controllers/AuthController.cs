using ITShop.API.Entities;
using ITShop.API.Interface;
using ITShop.API.Services;
using ITShop.API.ViewModels.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ITShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        private IAuthService _authService;

        public AuthController(UserManager<User> userManager, IAuthService _authService)
        {
            _userManager = userManager;

            this._authService = _authService;
        }
        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Login(LoginVM login, CancellationToken cancellationToken)
        {
            var message = await _authService.Login(login, cancellationToken);
            if (!message.IsValid)
                return Unauthorized(message);

            (SessionVM Session, string RefreshToken) authData = ((SessionVM, string))message.Data;

            Response.Cookies.Append("X-Refresh-Token", authData.RefreshToken, new CookieOptions() { HttpOnly = false, SameSite = SameSiteMode.None, Secure = true, IsEssential = true });

            return Ok(authData.Session);
        }
        [Authorize, HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            await _authService.LogoutAsync(user);
            return Ok();
        }
        [HttpGet("check2FA")]
        public async Task<IActionResult> Check2FA(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return BadRequest("Invalid username.");

            bool status = await _userManager.GetTwoFactorEnabledAsync(user);

            return Ok(status);
        }
        [HttpPost("update-2fa")]
        public async Task<IActionResult> EnableTwoFactorAuthentication(string username, bool status)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return BadRequest("Invalid username.");

            await _userManager.SetTwoFactorEnabledAsync(user, status);

            return Ok();
        }

        [HttpPost("generate-new-code")]
        public async Task<IActionResult> GenerateNewVerificationCode(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
                return BadRequest("Invalid username.");

            await _authService.GenerateVerificationCodeAsync(user);

            return Ok();
        }

        //[HttpGet("verify-2fa")]
        //public async Task<IActionResult> VerifyCode([FromBody] VerifyCodeRequestVM model)
        //{
        //    var user = await _userManager.FindByNameAsync(model.UserName);
        //    if (user == null)
        //        return BadRequest("Invalid username.");

        //    var isCodeValid = await _authService.VerifyCodeAsync(user, model.Code);
        //    if (!isCodeValid)
        //        return BadRequest("Invalid verification code.");

        //    // Code is valid, proceed with the desired actions

        //    return Ok();
        //}
    }
}
