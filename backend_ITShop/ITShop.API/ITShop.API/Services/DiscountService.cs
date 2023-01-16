using ITShop.API.Database;
using ITShop.API.Entities;
using ITShop.API.Enums;
using ITShop.API.Helper;
using ITShop.API.Interface;
using ITShop.API.ViewModels.Discount;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace ITShop.API.Services
{
    public class DiscountService : IDiscountService
    {
        public readonly ITShop_DBContext _dBContext;
        private UserManager<User> UserManager { get; set; }
        public IAuthContext AuthContext { get; set; }

        public DiscountService(ITShop_DBContext dBContext, UserManager<User>userManager, IAuthContext authContext)
        {
            _dBContext = dBContext;
            UserManager= userManager; 
            AuthContext = authContext;
        }

        public async Task<Message> GetAllPaged(int items_per_page = 10, int page_number = 1)
        {
            var data = _dBContext.Discounts
               
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
            var entity = await _dBContext.Discounts
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
        public async Task<Message> Snimi(DiscountSnimiVM x)
        {
            Discount? entity;
            if (x.Id == 0)
            {
                entity=new Discount();
                _dBContext.Add(entity);
            }
            else
            {
                entity = await _dBContext.Discounts.FirstOrDefaultAsync(s=>s.Id==x.Id);
                if (entity == null)
                    return new Message
                    {
                        IsValid = false,
                        Info = "Entitet nije pronađen.",
                        Status = ExceptionCode.NotFound
                    };
            }
            entity.Name = x.Name;
            entity.DiscountPercent = x.DiscountPercent;
            entity.Description = x.Description;
           

            await _dBContext.SaveChangesAsync();
            return new Message
            {
                IsValid = true,
                Info = $"Entitet {(x.Id == 0 ? "kreiran" : "ažuriran")}",
                Status = ExceptionCode.Success,
                Data = entity
            };
        }
        public async Task<Message> Delete (int id)
        {
            Discount? entity = await _dBContext.Discounts.FindAsync(id);
            if(entity is null)
            {
                return new Message
                {
                    Status=ExceptionCode.NotFound,
                    Info="entitet nije pronadjen",
                    IsValid=false
                };
            }
            _dBContext.Remove(entity);
            await _dBContext.SaveChangesAsync(); 
            return new Message
            {
                Status=ExceptionCode.Success,
                Info="Entite uspjesno izbrisan",
                IsValid=true
            };
        }

    }
}
