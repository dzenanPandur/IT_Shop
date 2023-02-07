using ITShop.API.Database;
using ITShop.API.Entities;
using ITShop.API.Enums;
using ITShop.API.Interface;
using ITShop.API.ViewModels.Role;
using ITShop.API.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ITShop.API.Services
{
    public class RoleService : IRoleService
    {
        public readonly ITShop_DBContext _dbContext;
        private readonly RoleManager<Role> _roleManager;
        private UserManager<User> UserManager { get; set; }
        public RoleService(ITShop_DBContext dbContext, RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
            UserManager = userManager;
        }

        public async Task<Message> GetRolesAsMessageAsync(CancellationToken cancellationToken)
        {
            try
            {
                var roles = await _dbContext.Roles.Where(x => x.IsDeleted == false).ToListAsync();
                var list = new List<UserRoleGetVM>();
                foreach (var role in roles)
                {
                    UserRoleGetVM newRole = new UserRoleGetVM
                    {
                        RoleId = role.Id,
                        RoleName = role.Name
                    };
                    list.Add(newRole);


                }
                return new Message
                {
                    IsValid = true,
                    Info = "Successfully got roles",
                    Status = ExceptionCode.Success,
                    Data = list
                };

            }
            catch (Exception ex)
            {
                return new Message
                {
                    IsValid = false,
                    Info = ex.Message,
                    Status = ExceptionCode.BadRequest
                };
            }
        }

        public async Task<Message> CreateRoleAsMessageAsync(RoleCreateVM roleCreateVM, CancellationToken cancellationToken)
        {
            var _role = await _dbContext.Roles.Where(x => x.Name == roleCreateVM.Name).FirstOrDefaultAsync(cancellationToken);
            if (_role == null)
            {
                var role = new Role()
                {
                    Name = roleCreateVM.Name,
                    Description = roleCreateVM.Description,
                };
                await _roleManager.CreateAsync(role);
                return new Message
                {
                    IsValid = true,
                    Data = role,
                    Status = ExceptionCode.Success
                };

            }

            return new Message
            {
                Info = "Ista uloga ne može biti dodana više puta!",
                IsValid = false,
                Status = ExceptionCode.BadRequest
            };

        }

        public async Task<Message> DeleteRoleAsMessageAsync(Guid roleId, CancellationToken cancellationToken)
        {
            var role = await _dbContext.Roles.FirstOrDefaultAsync(x => x.Id == roleId && !x.IsDeleted);
            if (role == null)
            {
                return new Message
                {
                    Info = "Uloga ne postoji!",
                    IsValid = false,
                    Status = ExceptionCode.NotFound
                };
            }
            role.IsDeleted = true;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return new Message
            {
                Info = "Uloga uspješno obrisana",
                IsValid = true,
                Status = ExceptionCode.Success
            };
        }

        public async Task<Message> AddRoleToUserAsMessageAsync(UserRoleVM userRoleVM, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.Where(x => x.Id == userRoleVM.UserId).FirstOrDefaultAsync(cancellationToken);

            if (user != null)
            {
                try
                {
                    var roles = await _dbContext.Roles
                             .Where(x => userRoleVM.RoleIds.Contains(x.Id))
                             .ToListAsync(cancellationToken);

                    await UserManager.AddToRolesAsync(user, roles.Select(x => x.NormalizedName)); //Zanemarit će već postojeće
                    return new Message
                    {
                        IsValid = true,
                        Info = "Successfuly added roles",
                        Status = ExceptionCode.Success
                    };
                }
                catch (Exception ex)
                {
                    return new Message
                    {
                        IsValid = false,
                        Info = "Bad request",
                        Status = ExceptionCode.BadRequest
                    };
                }


            }

            return new Message
            {
                Info = "Korisnik ne postoji u bazi!",
                IsValid = false,
                Status = ExceptionCode.BadRequest
            };
        }

        public async Task<Message> UpdateRoleToUserAsMessageAsync(RoleUpdateVM userRoleVM, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.Where(x => x.Id == userRoleVM.UserId).FirstOrDefaultAsync(cancellationToken);

            if (user != null)
            {
                try
                {
                    var record = await _dbContext.UserRoles.Where(x=>x.UserId == userRoleVM.UserId).FirstOrDefaultAsync(cancellationToken);

                     _dbContext.UserRoles.Remove(record);

                    var newRole = new IdentityUserRole<Guid>()
                    {
                        RoleId = userRoleVM.RoleId,
                        UserId = userRoleVM.UserId,
                    };
                    await _dbContext.AddAsync(newRole,cancellationToken);
                    await _dbContext.SaveChangesAsync(cancellationToken);

                    return new Message
                    {
                        IsValid = true,
                        Info = "Successfuly updated role",
                        Status = ExceptionCode.Success
                    };
                }
                catch (Exception ex)
                {
                    return new Message
                    {
                        IsValid = false,
                        Info = "Bad request",
                        Status = ExceptionCode.BadRequest
                    };
                }


            }

            return new Message
            {
                Info = "Korisnik ne postoji u bazi!",
                IsValid = false,
                Status = ExceptionCode.BadRequest
            };
        }
    }
}
