using ITShop.API.Database;
using ITShop.API.Entities;
using ITShop.API.Enums;
using ITShop.API.Interface;
using ITShop.API.ViewModels.CartItems;
using ITShop.API.ViewModels.Discount;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ITShop.API.Services
{
    public class CartItemsService: ICartItemsService
    {
        public readonly ITShop_DBContext _dBContext;
        private UserManager<User> UserManager { get; set; }
        public IAuthContext AuthContext { get; set; }


        public CartItemsService(ITShop_DBContext dBContext, UserManager<User> userManager, IAuthContext authContext)
        {
            _dBContext = dBContext;
            UserManager = userManager;
            AuthContext = authContext;
        }
        public async Task<Message> GetAllPaged(int items_per_page = 10, int page_number = 1)
        {
            var UserID = (await AuthContext.GetLoggedUser()).Id;

            var data = _dBContext.CartItems.Include(x=>x.Product).Include(x=>x.Product.ProductPictures)
                .Where(x=>x.UserID == UserID )
                .OrderByDescending(s => s.Id).AsQueryable();

            
            return new Message
            {
                IsValid = true,
                Info = "Successfully got entities",
                Status = ExceptionCode.Success,
                Data = data
            };
        }

        public async Task<Message> Get(int id)
        {
            var entity = await _dBContext.CartItems
                .Include(x=>x.Product)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (entity is null)
            {
                return new Message
                {
                    IsValid = false,
                    Info = "Entitet nije pronađen",
                    Status = ExceptionCode.NotFound,
                };
            }

            return new Message
            {
                IsValid = true,
                Info = "Entitet pronađen",
                Status = ExceptionCode.Success,
                Data = entity
            };
        }
        public async Task<Message> Snimi(CartItemsSnimiVM x)
        {
            CartItems? entity;
            var LoggedUserId = (await AuthContext.GetLoggedUser()).Id;

            entity = await _dBContext.CartItems.FirstOrDefaultAsync(s => s.UserID == LoggedUserId &&
                                                                         s.ProductID == x.ProductID);
            if(entity == null)
            {
                entity = new CartItems();
                _dBContext.Add(entity);
                entity.Quantity = x.Quantity;
                entity.TotalPrice = x.TotalPrice;
                entity.ProductID = x.ProductID;
                entity.UserID = LoggedUserId;
            }
            else
            {
                entity.Quantity += x.Quantity;
                entity.TotalPrice += x.TotalPrice;
            }

            await _dBContext.SaveChangesAsync();
            return new Message
            {
                IsValid = true,
                Info = $"Entitet ažuriran",
                Status = ExceptionCode.Success,
                Data = entity
            };
        }
        public async Task<Message> Update(int id,CartItemsSnimiVM updatedCartItem)
        {
            CartItems? entity;
            var LoggedUserId = (await AuthContext.GetLoggedUser()).Id;

            entity = await _dBContext.CartItems.FirstOrDefaultAsync(s => s.UserID == LoggedUserId &&
                                                                         s.ProductID == updatedCartItem.ProductID);
            if (entity != null)
            {
                id = entity.Id;
                entity.Quantity = updatedCartItem.Quantity;
                entity.TotalPrice = updatedCartItem.TotalPrice;
                entity.ProductID = updatedCartItem.ProductID;
                entity.UserID = LoggedUserId;
            }
            

            await _dBContext.SaveChangesAsync();
            return new Message
            {
                IsValid = true,
                Info = $"Entitet ažuriran",
                Status = ExceptionCode.Success,
                Data = entity
            };
        }
        public async Task<Message> Delete(int id)
        {
            CartItems? entity = await _dBContext.CartItems.FindAsync(id);
            if (entity is null)
            {
                return new Message
                {
                    Status = ExceptionCode.NotFound,
                    Info = "entitet nije pronadjen",
                    IsValid = false
                };
            }
            _dBContext.Remove(entity);
            await _dBContext.SaveChangesAsync();
            return new Message
            {
                Status = ExceptionCode.Success,
                Info = "Entite uspjesno izbrisan",
                IsValid = true
            };
        }
    }
}
