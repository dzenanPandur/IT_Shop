using Grpc.Core;
using ITShop.API.Database;
using ITShop.API.Entities;
using ITShop.API.Enums;
using ITShop.API.Helper;
using ITShop.API.Interface;

using ITShop.API.ViewModels.User;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Management.ContainerInstance.Fluent.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;



namespace ITShop.API.Services
{
    public class ProductService : IProductService
    {
        public readonly ITShop_DBContext _dbContext;
        private UserManager<User> UserManager { get; set; }
        public IAuthContext AuthContext { get; set; }

        public ProductService(ITShop_DBContext dbContext, UserManager<User> userManager, IAuthContext authContext)
        {
            _dbContext = dbContext;
            UserManager = userManager;
            AuthContext = authContext;
        }

        public async Task<Message> GetAllPaged(ProductGetVM vm, int items_per_page = 10, int page_number = 1)
        {
            var data = _dbContext.Products
                .Where(x => vm.Name == null || x.Name.StartsWith(vm.Name))
                .Where(x => vm.PriceMin == null || x.Price >= vm.PriceMin)
                .Where(x => vm.PriceMax == null || x.Price <= vm.PriceMax)
                .Where(x => vm.CategoryID == null || x.CategoryID == vm.CategoryID)
                .Include(x=>x.ProductPictures)
                .OrderByDescending(s => s.Id)
                .AsQueryable();

            var list = await PagedList<Product>.Create(data, page_number, items_per_page);
            
            return new Message
            {
                IsValid = true,
                Info = "Successfully got entities",
                Status = ExceptionCode.Success,
                Data = list
            };
        }

        public async Task<Message> Get(int id)
        {
            var entity = await _dbContext.Products
                .Include(x => x.Discount)
                .Include(x => x.ProductCategory)
                .Include(x => x.ProductInventory.Location)
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


        


        //[HttpPost]
        //public ActionResult Create(EventModel eventmodel, HttpPostedFileBase file)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var originalFilename = Path.GetFileName(file.FileName);
        //        string fileId = Guid.NewGuid().ToString().Replace("-", "");
        //        string userId = GetUserId(); // Function to get user id based on your schema

        //        string path = Path.Combine(Server.MapPath("~/Uploads/Photo/"), userId, fileId);
        //        file.SaveAs(path);

        //        eventmodel.ImageId = fileId;
        //        eventmodel.OriginalFilename = originalFilename;

        //        _dbContext.EventModels.AddObject(eventmodel);
        //        _dbContext.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(eventmodel);
        //}


        public async Task<Message> Snimi(ProductSnimiVM x)
        {
            Product? entity;
            if (x.Id == 0)
            {
                entity = new Product();
                _dbContext.Add(entity);
            }
            else
            {
                entity = await _dbContext.Products.FirstOrDefaultAsync(s => s.Id == x.Id);
                if (entity == null)
                    return new Message
                    {
                        IsValid = false,
                        Info = "Entitet nije pronađen.",
                        Status = ExceptionCode.NotFound
                    };
            }

            entity.Name = x.Name;
            entity.Price = x.Price;
            entity.Description = x.Description;
            entity.InventoryID = x.InventoryID;
            entity.CategoryID = x.CategoryID;
            
            entity.DiscountID = x.DiscountID;

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
            Product? entity = await _dbContext.Products.FindAsync(id);

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
