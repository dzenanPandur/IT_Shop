using ITShop.API.Database;
using ITShop.API.Entities;
using ITShop.API.Enums;
using ITShop.API.Interface;
using ITShop.API.ViewModels.Discount;
using ITShop.API.ViewModels.Review;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ITShop.API.Services
{
    public class ReviewService:IReviewsService
    {
        public readonly ITShop_DBContext _dBContext;
        private UserManager<User> UserManager { get; set; }
        public IAuthContext AuthContext { get; set; }

        public ReviewService(ITShop_DBContext dBContext, UserManager<User> userManager, IAuthContext authContext)
        {
            _dBContext = dBContext;
            UserManager = userManager;
            AuthContext = authContext;
        }

        public async Task<Message> Snimi(ReviewSnimiVM x)
        {
            if(!(x.ReviewValue >= 1 && x.ReviewValue <= 5))
            {
                return new Message
                {
                    IsValid = false,
                    Info = "Cannot rate a product with 0 stars",
                    Status = ExceptionCode.BadRequest
                };
            }

            var product = await _dBContext.Products.FindAsync(x.ProductID);
            if (product == null)
            {
                return new Message
                {
                    IsValid = false,
                    Info = "Proizvod nije pronađen.",
                    Status = ExceptionCode.NotFound
                };
            }


            var user = await AuthContext.GetLoggedUser();
            if (user == null)
            {
                return new Message
                {
                    IsValid = false,
                    Info = "Korisnik nije prijavljen.",
                    Status = ExceptionCode.Unauthorized
                };
            }

            var orderExists = await _dBContext.Orders
            .AnyAsync(o => o.UserID == user.Id && o.OrderItems.Any(oi => oi.ProductID == x.ProductID));

            if (!orderExists)
            {
                return new Message
                {
                    IsValid = false,
                    Info = "Nemate pravo ocijeniti ovaj proizvod jer nije sadržan u vašoj narudžbi.",
                    Status = ExceptionCode.Forbidden
                };
            }


            var existingReview = await _dBContext.Reviews
            .FirstOrDefaultAsync(r => r.UserID == user.Id && r.ProductID == x.ProductID);


            Review newReview;

            if (existingReview == null)
            {
                 newReview = new Review
                {
                    UserID = user.Id,
                    ProductID = x.ProductID,
                    ReviewValue = x.ReviewValue,
                    ReviewText = x.ReviewText,
                    ReviewDate = DateTime.Now
                };

                _dBContext.Add(newReview);
            }

            else
            {
                existingReview.ReviewValue = x.ReviewValue;
                existingReview.ReviewText = x.ReviewText;
                existingReview.ReviewDate = DateTime.Now;
                newReview = existingReview;
            }

            await _dBContext.SaveChangesAsync();

            return new Message
            {
                IsValid = true,
                Info = existingReview == null ? "Entitet kreiran." : "Entitet ažuriran.",
                Status = ExceptionCode.Success,
                Data = existingReview ?? newReview
            };
        }
        public async Task<Message> Delete(int id)
        {
            Review? entity = await _dBContext.Reviews.FindAsync(id);
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
