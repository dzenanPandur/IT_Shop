using ITShop.API.Database;
using ITShop.API.Entities;
using ITShop.API.Enums;
using ITShop.API.Interface;
using ITShop.API.ViewModels.Role;
using ITShop.API.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace ITShop.API.Services
{
    public class UserService : IUserService
    {
        public readonly ITShop_DBContext _dbContext;
        private UserManager<User> UserManager { get; set; }
        public IAuthContext authContext { get; set; }

        public UserService(ITShop_DBContext dbContext, UserManager<User> userManager, IAuthContext authContext)
        {
            _dbContext = dbContext;
            UserManager = userManager;
            this.authContext = authContext;
        }

        public async Task<Message> CreateUserAsMessageAsync(UserCreateVM userCreateVM, CancellationToken cancellationToken)
        {
            var status = ExceptionCode.Success;
            try
            {
                bool duplicateEmail = await ThrowOnCreateUserDuplicateEmailError(userCreateVM, cancellationToken);
                bool duplicateUserName = await ThrowOnCreateDuplicateUsernameError(userCreateVM, cancellationToken);
                if (duplicateEmail && duplicateUserName)
                {
                    status = ExceptionCode.BadRequest;
                    throw new Exception($"Korisnik sa e-mailom: '{userCreateVM.Email}' već postoji!" + Environment.NewLine + $"Korisnik sa username: '{userCreateVM.UserName}' već postoji!");
                }

                if (duplicateEmail)
                {
                    status = ExceptionCode.BadRequest;
                    throw new Exception($"Korisnik sa e-mailom: '{userCreateVM.Email}' već postoji!");
                }

                if (duplicateUserName)
                {
                    status = ExceptionCode.BadRequest;
                    throw new Exception($"Korisnik sa username: '{userCreateVM.UserName}' već postoji");
                }
                using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
                try
                {

                    var user = new User();
                    user.Email = userCreateVM.Email;
                    user.UserName = userCreateVM.UserName;
                    user.PhoneNumber = userCreateVM.PhoneNumber;
                    user.LastName = userCreateVM.LastName;
                    user.FirstName = userCreateVM.FirstName;
                    user.Gender = userCreateVM.Gender;
                    user.CreatedDate = DateTime.Now;
                    user.IsDeleted = false;

                    await UserManager.CreateAsync(user, userCreateVM.Password);
                    var roles = await _dbContext.Roles
                           .Where(x => userCreateVM.UserRoles.Contains(x.Id))
                           .ToListAsync(cancellationToken);

                    await UserManager.AddToRolesAsync(user, roles.Select(x => x.NormalizedName));

                    await _dbContext.SaveChangesAsync(cancellationToken);

                    await transaction.CommitAsync(cancellationToken);


                    return new Message { Info = "Successfully created user", IsValid = true, Status = status };
                }
                catch (Exception ex)
                {
                    status = ExceptionCode.BadRequest;
                    await transaction.RollbackAsync(cancellationToken);
                    return new Message { Info = ex.Message, IsValid = false, Status = status };
                }
            }
            catch (Exception ex)
            {
                status = ExceptionCode.BadRequest;
                return new Message { Info = ex.Message, IsValid = false, Status = status };
            }
        }

        private async Task<bool> ThrowOnCreateUserDuplicateEmailError(UserCreateVM userCreateVM, CancellationToken cancellationToken)
        {
            return await _dbContext.Users.
                AnyAsync(x => x.Email == userCreateVM.Email, cancellationToken);
        }

        private async Task<bool> ThrowOnCreateDuplicateUsernameError(UserCreateVM userCreateVM, CancellationToken cancellationToken)
        {
            return await _dbContext.Users.
                AnyAsync(x => x.UserName == userCreateVM.UserName, cancellationToken);
        }

        public async Task<Message> GetUsersAsMessageAsync(CancellationToken cancellationToken)
        {
            try
            {
                var loggedUser = await authContext.GetLoggedUser();

                var users = await _dbContext.Users.Where(x => x.Id != loggedUser.Id).ToListAsync();

                var userRoles = await _dbContext.UserRoles.ToListAsync(cancellationToken);
                var list = new List<UserGetVM>();
                var roles = await _dbContext.Roles.ToListAsync(cancellationToken);
                foreach (var user in users)
                {
                    var x = new List<UserRoleGetVM>();

                    UserGetVM newUser = new UserGetVM
                    {
                        Email = user.Email,
                        FirstName = user.FirstName,
                        Gender = user.Gender,
                        Id = user.Id,
                        LastName = user.LastName,
                        PhoneNumber = user.PhoneNumber,
                        UserName = user.UserName
                    };

                    // can u try now
                    x = userRoles.
                        Where(x => x.UserId == user.Id)
                        .Select(x => new UserRoleGetVM
                        {
                            RoleId = x.RoleId,
                            RoleName = roles.Where(a => a.Id == x.RoleId).FirstOrDefault().Name
                        }).ToList();
                    newUser.Role = x;
                    list.Add(newUser);


                }
                return new Message
                {
                    IsValid = true,
                    Info = "Successfully got users",
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

        //public bool _SendMail(string To, string Subject, string Sadrzaj)
        //{
        //    MailMessage message = new MailMessage("mverifikacija@gmail.com", To);
        //    message.Subject = Subject;
        //    message.Body = Sadrzaj;
        //    message.BodyEncoding = Encoding.UTF8;
        //    message.IsBodyHtml = true;
        //    SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
        //    System.Net.NetworkCredential basicCredential1 = new
        //    System.Net.NetworkCredential("mverifikacija@gmail.com", "qhmjyeiyiuruotrc");
        //    client.EnableSsl = true;
        //    client.UseDefaultCredentials = false;
        //    client.Credentials = basicCredential1;

        //    try
        //    {
        //        client.Send(message);
        //        return true;
        //    }

        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

    }
}
