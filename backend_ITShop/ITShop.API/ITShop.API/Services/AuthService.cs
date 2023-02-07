using ITShop.API.Database;
using ITShop.API.Entities;
using ITShop.API.Enums;
using ITShop.API.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITShop.API.Helper;
using ITShop.API.ViewModels.Auth;

namespace ITShop.API.Services
{
    public class AuthService : IAuthService
    {
        private JwtConfiguration _jwtConfiguration { get; set; }
        private UserManager<User> _userManager { get; set; }
        private SignInManager<User> _signInManager { get; set; }
        private ITShop_DBContext _dbContext { get; set; }
        private IAuthContext authContext { get; set; }

        public AuthService(JwtConfiguration jwtConfiguration,
         UserManager<User> userManager,
         SignInManager<User> signInManager,
         ITShop_DBContext dbContext, IAuthContext authContext)
        {
            _jwtConfiguration = jwtConfiguration;
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
            this.authContext = authContext;
        }

        public async Task<Message> Login(LoginVM loginVM, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == loginVM.Username, cancellationToken);

            if (user is null)
            {
                return new Message
                {
                    Info = "Forbidden",
                    IsValid = false,
                    Status = ExceptionCode.Forbidden
                };
            }

            var userSignInResult = await _userManager.CheckPasswordAsync(user, loginVM.Password);

            if (userSignInResult)
            {
                var roles = await _userManager.GetRolesAsync(user);

                //var permissions = await GetPermissionsByRoles(roles, cancellationToken);

                (string accessToken, long expiresIn) = GenerateJwt(user, roles);

                var refreshToken = SetRefreshToken(user);
                await _dbContext.SaveChangesAsync(cancellationToken);

                var session = await GetSession(user.Id, accessToken, expiresIn, cancellationToken);

                return new Message
                {
                    Info = "Success",
                    IsValid = true,
                    Status = ExceptionCode.Success,
                    Data = (session, refreshToken)
                };

            }

            return new Message
            {
                Info = "Forbidden",
                IsValid = false,
                Status = ExceptionCode.Forbidden
            };
        }

        public async Task<Message> RefreshToken(string refreshToken, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                return new Message
                {
                    Info = "Forbidden",
                    IsValid = false,
                    Status = ExceptionCode.Forbidden
                };
            }

            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshToken == refreshToken, cancellationToken);
            if (user is null)
            {
                return new Message
                {
                    Info = "Forbidden",
                    IsValid = false,
                    Status = ExceptionCode.Forbidden
                };
            }
            else if (user.RefreshTokenExpireDate <= DateTime.UtcNow)
            {
                user.RefreshToken = null;
                user.RefreshTokenExpireDate = DateTime.MinValue;
                await _dbContext.SaveChangesAsync(CancellationToken.None);

                return new Message
                {
                    Info = "Forbidden",
                    IsValid = false,
                    Status = ExceptionCode.Forbidden
                };
            }

            var roles = await _userManager.GetRolesAsync(user);

            //var permissions = await GetPermissionsByRoles(roles, cancellationToken);

            (string accessToken, long expiresIn) = GenerateJwt(user, roles);

            var session = await GetSession(user.Id, accessToken, expiresIn, cancellationToken);

            return new Message
            {
                Info = "Success",
                IsValid = true,
                Status = ExceptionCode.Success,
                Data = session
            };
        }

        private (string Token, long ExpiresIn) GenerateJwt(User user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            // Adding role claims. 
            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x));
            claims.AddRange(roleClaims);



            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_jwtConfiguration.ExpirationAccessTokenInMinutes));

            var token = new JwtSecurityToken(
                issuer: _jwtConfiguration.Issuer,
                audience: _jwtConfiguration.Issuer,
                (IEnumerable<System.Security.Claims.Claim>)claims,
                expires: expires,
                signingCredentials: creds
            );

            return (new JwtSecurityTokenHandler().WriteToken(token), new DateTimeOffset(expires).ToUnixTimeSeconds());
        }

        private string SetRefreshToken(User user)
        {
            user.RefreshToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            user.RefreshTokenExpireDate = DateTime.UtcNow.AddMinutes(_jwtConfiguration.ExpirationRefreshTokenInMinutes);
            return user.RefreshToken;
        }

        public async Task<Message> LogoutAsync(User user)
        {
            await _signInManager.SignOutAsync();

            try
            {
                user.RefreshToken = null;
                user.RefreshTokenExpireDate = DateTime.MinValue;
                await _dbContext.SaveChangesAsync(CancellationToken.None);
            }
            catch(Exception ex)
            {
                return new Message
                {
                    Info = ex.Message,
                    IsValid = false,
                    Status = ExceptionCode.BadRequest,
                };
            }

            return new Message
            {
                Info = "Success",
                IsValid = true,
                Status = ExceptionCode.Success,
            };
        }

        private async Task<SessionVM> GetSession(Guid userId, string accessToken, long accessTokenExpiration, CancellationToken cancellationToken)
        {
            var websiteUser = await _dbContext.Users
                .Select(x => new
                {
                    UserId = x.Id,
                    x.FirstName,
                    x.LastName,
                    PhoneNumber = x.PhoneNumber,
                    Email = x.Email,
                    Gender = x.Gender
                })
                .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

            var roles = await _dbContext.UserRoles.Where(x => x.UserId == userId).Select(x => x.RoleId).ToListAsync(cancellationToken);
            List<string> stringRoles = await _dbContext.Roles.Where(x => roles.Contains(x.Id)).Select(x => x.Name).ToListAsync(cancellationToken);

            var session = new SessionVM()
            {
                UserId = userId,
                FirstName = websiteUser?.FirstName,
                LastName = websiteUser?.LastName,
                Token = accessToken,
                TokenExpireDate = accessTokenExpiration,
                PhoneNumber = websiteUser.PhoneNumber,
                Email = websiteUser.Email,
                Gender = websiteUser.Gender,
                Roles = stringRoles,
            };

            return session;
        }
    }
}
