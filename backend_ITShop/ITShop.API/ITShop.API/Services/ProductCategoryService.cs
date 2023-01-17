using ITShop.API.Database;
using ITShop.API.Entities;
using ITShop.API.Enums;
using ITShop.API.Interface;
using ITShop.API.ViewModels.Location;
using ITShop.API.ViewModels.ProductCategory;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ITShop.API.Services
{
    public class ProductCategoryService : IProductCategoryService
    {
        public readonly ITShop_DBContext _dBContext;
        private UserManager<User> UserManager { get; set; }
        public IAuthContext AuthContext { get; set; }

        public ProductCategoryService(ITShop_DBContext dBContext, UserManager<User> userManager, IAuthContext authContext)
        {
            _dBContext = dBContext;
            UserManager = userManager;
            AuthContext = authContext;
        }
        public async Task<Message> GetAllPaged(int items_per_page = 10, int page_number = 1)
        {
            var data = _dBContext.ProductCategories

                .OrderByDescending(s => s.Id).AsQueryable();

            //var list = await PagedList<Discount>.Create(data, items_per_page, page_number);
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
            var entity = await _dBContext.ProductCategories
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
        public async Task<Message> Snimi(ProductCategorySnimiVM x)
        {
            ProductCategory? entity;
            if (x.Id == 0)
            {
                entity = new ProductCategory();
                _dBContext.Add(entity);
            }
            else
            {
                entity = await _dBContext.ProductCategories.FirstOrDefaultAsync(s => s.Id == x.Id);
                if (entity == null)
                    return new Message
                    {
                        IsValid = false,
                        Info = "Entitet nije pronađen.",
                        Status = ExceptionCode.NotFound
                    };
            }
            entity.Name = x.Name;

            await _dBContext.SaveChangesAsync();
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
            ProductCategory? entity = await _dBContext.ProductCategories.FindAsync(id);
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
