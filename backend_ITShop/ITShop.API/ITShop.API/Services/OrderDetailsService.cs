using ITShop.API.Database;
using ITShop.API.Entities;
using ITShop.API.Enums;
using ITShop.API.Helper;
using ITShop.API.Interface;
using ITShop.API.ViewModels.OrderDetails;
using ITShop.API.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ITShop.API.Services
{
    public class OrderDetailsService: IOrderDetailsService
    {
        public readonly ITShop_DBContext _dbContext;
        private UserManager<User> UserManager { get; set; }
        public IAuthContext AuthContext { get; set; }

        public OrderDetailsService(ITShop_DBContext dbContext, UserManager<User> userManager, IAuthContext authContext)
        {
            _dbContext = dbContext;
            UserManager = userManager;
            AuthContext = authContext;
        }

        public async Task<Message> GetAllPaged(int items_per_page = 10, int page_number = 1)
        {
            var data = _dbContext.OrderDetails
                .Include(x => x.Product)
                .Include(x => x.Discount)
                .Include(x => x.Order)
                .OrderByDescending(s => s.Id)
                .AsQueryable();

            //var list = await PagedList<Product>.Create(page_number, items_per_page);

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
            var entity = await _dbContext.OrderDetails
                .Include(x=>x.Product)
                .Include(x=>x.Discount)
                .Include(x=>x.Order)
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

        public async Task<Message> Snimi(OrderDetailsSnimiVM x)
        {
            OrderDetails? entity;
            if (x.Id == 0)
            {
                entity = new OrderDetails();
                _dbContext.Add(entity);
            }
            else
            {
                entity = await _dbContext.OrderDetails.FirstOrDefaultAsync(s => s.Id == x.Id);
                if (entity == null)
                    return new Message
                    {
                        IsValid = false,
                        Info = "Entitet nije pronađen.",
                        Status = ExceptionCode.NotFound
                    };
            }
            entity.UnitPrice = x.UnitPrice;
            entity.Quantity=x.Quantity;
            entity.DiscountID = x.DiscountID;
            entity.OrderID = x.OrderID;
            entity.ProductID = x.ProductID;

            await _dbContext.SaveChangesAsync();
            return new Message
            {
                IsValid = true,
                Info = $"Entitet {(x.Id == 0 ? "kreiran" : "ažuriran")}",
                Status = ExceptionCode.Success,
                Data = entity
            };
        }

        public async Task<Message> Delete(int id)
        {
            OrderDetails? entity = await _dbContext.OrderDetails.FindAsync(id);

            if (entity == null)
                return new Message
                {
                    IsValid = false,
                    Info = "Entitet nije pronađen.",
                    Status = ExceptionCode.NotFound
                };

            _dbContext.Remove(entity);

            await _dbContext.SaveChangesAsync();
            return new Message
            {
                IsValid = true,
                Info = $"Entitet obrisan.",
                Status = ExceptionCode.Success,
                Data = entity
            };
        }
    }
}
