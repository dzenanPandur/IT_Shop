using ITShop.API.Database;
using ITShop.API.Entities;
using ITShop.API.Enums;
using ITShop.API.Interface;
using ITShop.API.ViewModels.Role;
using ITShop.API.ViewModels.Subscription;
using ITShop.API.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.EntityFrameworkCore;

namespace ITShop.API.Services
{
    public class SubscriptionService:ISubscriptionService
    {
        public readonly ITShop_DBContext _dbContext;

        public SubscriptionService(ITShop_DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Message> CreateSubscriptionAsMessageAsync(SubscriptionCreateVM subscriptionCreateVM, CancellationToken cancellationToken)
        {
            var _subscriber = await _dbContext.Subscriptions.Where(x => x.UserID==subscriptionCreateVM.UserId).FirstOrDefaultAsync(cancellationToken);
            if (_subscriber == null)
            {
                var subscriber = new Subscription()
                {
                    UserID = subscriptionCreateVM.UserId,
                    isSubscribed = subscriptionCreateVM.isSubscribed,
                };
                _dbContext.Add(subscriber);
                await _dbContext.SaveChangesAsync();
                return new Message
                {
                    IsValid = true,
                    Data = subscriber,
                    Status = ExceptionCode.Success
                };

            }

            return new Message
            {
                Info = "This user's subscription already exists!",
                IsValid = false,
                Status = ExceptionCode.BadRequest
            };

        }

        //public async Task<Message> DeleteSubscriptionAsMessageAsync(int id, CancellationToken cancellationToken)
        //{
        //    var subscription = await _dbContext.Subscriptions.FirstOrDefaultAsync(x => x.Id == id);
        //    if (subscription == null)
        //    {
        //        return new Message
        //        {
        //            Info = "Subscription ne postoji!",
        //            IsValid = false,
        //            Status = ExceptionCode.NotFound
        //        };
        //    }
            

        //    await _dbContext.SaveChangesAsync(cancellationToken);
        //    return new Message
        //    {
        //        Info = "Subscription uspješno obrisan",
        //        IsValid = true,
        //        Status = ExceptionCode.Success
        //    };
        //}

        public async Task<Message> SubscriptionsGetAsMessageAsync(CancellationToken cancellationToken)
        {
            try
            {
                var subscriptions = await _dbContext.Subscriptions.ToListAsync();
                var list = new List<SubscriptionGetVM>();
                foreach (var subscription in subscriptions)
                {
                    SubscriptionGetVM newSubscription = new SubscriptionGetVM
                    {
                        Id = subscription.Id,
                        UserId=subscription.UserID,
                        isSubscribed=subscription.isSubscribed

                    };
                    list.Add(newSubscription);


                }
                return new Message
                {
                    IsValid = true,
                    Info = "Successfully got subscriptions",
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

        public async Task<Message> SubscriptionsGetByIdAsMessageAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _dbContext.Subscriptions
                    .FirstOrDefaultAsync(s => s.UserID == id);
                //var user = await _dbContext.Subscriptions.Where(x => x.Id == subscriptionUpdateVM.UserId).FirstOrDefaultAsync(cancellationToken);
                if (entity == null)
                {
                    var subscriber = new Subscription()
                    {
                        UserID = id,
                        isSubscribed = false,
                    };
                    _dbContext.Add(subscriber);
                    await _dbContext.SaveChangesAsync();
                    return new Message
                    {
                        IsValid = true,
                        Data = subscriber,
                        Status = ExceptionCode.Success
                    };
                }
                var subscriptions = await _dbContext.Subscriptions.Where(x=>x.UserID == id).ToListAsync();
                var list = new List<SubscriptionGetVM>();
                foreach (var subscription in subscriptions)
                {
                    SubscriptionGetVM newSubscription = new SubscriptionGetVM
                    {
                        Id = subscription.Id,
                        UserId = subscription.UserID,
                        isSubscribed = subscription.isSubscribed

                    };
                    list.Add(newSubscription);


                }
                return new Message
                {
                    IsValid = true,
                    Info = "Successfully got subscriptions",
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

        public async Task<Message> SubscriptionUpdateAsMessageAsync(SubscriptionUpdateVM subscriptionUpdateVM, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.Where(x => x.Id == subscriptionUpdateVM.UserId).FirstOrDefaultAsync(cancellationToken);

            if (user != null)
            {
                try
                {
                    var record = await _dbContext.Subscriptions.Where(x => x.UserID == subscriptionUpdateVM.UserId).FirstOrDefaultAsync(cancellationToken);

                    _dbContext.Subscriptions.Remove(record);

                    var newSubscription = new Subscription()
                    {
                        UserID= subscriptionUpdateVM.UserId,
                        isSubscribed= subscriptionUpdateVM.isSubscribed
                    };
                    await _dbContext.AddAsync(newSubscription, cancellationToken);
                    await _dbContext.SaveChangesAsync(cancellationToken);

                    return new Message
                    {
                        IsValid = true,
                        Info = "Successfuly updated user's subscription!",
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
        public async Task<Message> SubscriptionsGetByIdEmpAsMessageAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _dbContext.Subscriptions.FirstOrDefaultAsync(s => s.UserID == id);

                if (entity == null)
                {
                    return new Message
                    {
                        IsValid = false,
                        Info = "Subscription not found",
                        Status = ExceptionCode.NotFound
                    };
                }

                var subscriptions = await _dbContext.Subscriptions.Where(x => x.UserID == id).ToListAsync();
                var list = new List<SubscriptionGetVM>();
                foreach (var subscription in subscriptions)
                {
                    SubscriptionGetVM newSubscription = new SubscriptionGetVM
                    {
                        Id = subscription.Id,
                        UserId = subscription.UserID,
                        isSubscribed = subscription.isSubscribed

                    };
                    list.Add(newSubscription);
                }
                return new Message
                {
                    IsValid = true,
                    Info = "Successfully got subscriptions",
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
    }
}
