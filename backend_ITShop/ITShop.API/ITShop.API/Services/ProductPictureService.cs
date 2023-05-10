using ITShop.API.Database;
using ITShop.API.Entities;
using ITShop.API.Enums;
using ITShop.API.Helper;
using ITShop.API.Interface;
using ITShop.API.ViewModels.Product;
using Microsoft.EntityFrameworkCore;



namespace ITShop.API.Services
{
    public class ProductPictureService : IProductPictureService
    {
        public readonly ITShop_DBContext _dbContext;

        public ProductPictureService(ITShop_DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Message> GetAll(int id)
        {
            var entity = await _dbContext.ProductPictures
                .Where(x => x.ProductID == id)
                .ToListAsync();
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
        public async Task<Message> Create(int id, ProductPictureAddVM x)
        {
            if (x.product_image is null)
            {
                return new Message
                {
                    IsValid = false,
                    Info = "Slika nije uploadovana",
                    Status = ExceptionCode.BadRequest,
                };
            }

            if (x.product_image.Length > 900 * 1000)
                return new Message
                {
                    IsValid = false,
                    Info = "max velicina fajla je 900 KB",
                    Status = ExceptionCode.BadRequest,
                };

            string ekstenzija = Path.GetExtension(x.product_image.FileName);

            var filename = $"{Guid.NewGuid()}{ekstenzija}";

            x.product_image.CopyTo(new FileStream(Config.SlikeFolder + filename, FileMode.Create));

            var slika = new ProductPicture
            {
                FileName = Config.SlikeURL + filename,
                FileSize = (int)x.product_image.Length,
                ProductID = id
            };
            _dbContext.Add(slika);
            await _dbContext.SaveChangesAsync();

            return new Message
            {
                IsValid = true,
                Info = "Slika uspjesno dodana",
                Status = ExceptionCode.Success,
                Data = slika
            };
        }
        public async Task<Message> Delete(int id)
        {
            ProductPicture? entity = await _dbContext.ProductPictures.FindAsync(id);

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
