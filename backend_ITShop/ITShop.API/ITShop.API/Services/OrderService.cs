using ITShop.API.Database;
using ITShop.API.Entities;
using ITShop.API.Enums;
using ITShop.API.Interface;
using ITShop.API.ViewModels.Discount;
using ITShop.API.ViewModels.Order;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ITShop.API.Services
{
    public class OrderService: IOrderService
    {
        public readonly ITShop_DBContext _dBContext;
        private UserManager<User> UserManager { get; set; }
        public IAuthContext AuthContext { get; set; }

        public OrderService(ITShop_DBContext dBContext, UserManager<User> userManager, IAuthContext authContext)
        {
            _dBContext = dBContext;
            UserManager = userManager;
            AuthContext = authContext;
        }
        public async Task<Message> GetAllPaged(int items_per_page = 10, int page_number = 1)
        {
            var data = _dBContext.Orders
                .Include(x => x.User).Include(x => x.OrderItems).ThenInclude(oi=>oi.Product).ThenInclude(p=>p.ProductPictures)
                .OrderByDescending(s => s.Id).AsQueryable();

            var orderData=new List<OrderVM>();

            foreach (var entity in data)
            {
                var orderVM = new OrderVM
                {
                    Id = entity.Id,
                    OrderDate = entity.OrderDate,
                    UserID = entity.UserID,
                    Payment_intent_id = entity.Payment_intent_id,
                    Receipt_url = entity.Receipt_url,
                    isSubscribed = entity.isSubscribed,
                    TotalTotalPrice = entity.TotalTotalPrice,
                    Quantity = entity.Quantity,
                    ShippingAdress = entity.ShippingAdress,
                    OrderItems = entity.OrderItems?.Select(orderItem => new OrderItemVM
                    {
                        Quantity = orderItem.Quantity,
                        TotalPrice = orderItem.TotalPrice,
                        ProductID = orderItem.ProductID,
                        OrderID = orderItem.OrderID
                    })
                };
                orderData.Add(orderVM);
            }
           
            // Serialize the data using the configured settings
           
            return new Message
            {
                IsValid = true,
                Info = "Successfully got entities",
                Status = ExceptionCode.Success,
                Data = data
            };
        }
        public async Task<Message> GetAllForUser(int items_per_page = 10, int page_number = 1)
        {

            var UserID = (await AuthContext.GetLoggedUser()).Id;
            var data = _dBContext.Orders
                .Include(x => x.User).Include(x => x.OrderItems).ThenInclude(oi => oi.Product).ThenInclude(p => p.ProductPictures)
                .OrderByDescending(s => s.Id).Where(x=>x.UserID==UserID).AsQueryable();

            var orderData = new List<OrderVM>();

            foreach (var entity in data)
            {
                var orderVM = new OrderVM
                {
                    Id = entity.Id,
                    OrderDate = entity.OrderDate,
                    UserID = entity.UserID,
                    Payment_intent_id = entity.Payment_intent_id,
                    Receipt_url = entity.Receipt_url,
                    isSubscribed = entity.isSubscribed,
                    TotalTotalPrice = entity.TotalTotalPrice,
                    Quantity = entity.Quantity,
                    ShippingAdress = entity.ShippingAdress,
                    OrderItems = entity.OrderItems?.Select(orderItem => new OrderItemVM
                    {
                        Quantity = orderItem.Quantity,
                        TotalPrice = orderItem.TotalPrice,
                        ProductID = orderItem.ProductID,
                        OrderID = orderItem.OrderID
                    })
                };
                orderData.Add(orderVM);
            }

            // Serialize the data using the configured settings

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
            var entity = await _dBContext.Orders
                .Include(x => x.User).Include(x => x.OrderItems).ThenInclude(oi => oi.Product).ThenInclude(p => p.ProductPictures)

                .FirstOrDefaultAsync(s => s.Id == id);

            var orderVM = new OrderVM
            {
                Id = entity.Id,
                OrderDate = entity.OrderDate,
                UserID = entity.UserID,
                Payment_intent_id = entity.Payment_intent_id,
                Receipt_url = entity.Receipt_url,
                isSubscribed = entity.isSubscribed,
                TotalTotalPrice = entity.TotalTotalPrice,
                Quantity = entity.Quantity,
                ShippingAdress = entity.ShippingAdress,
                OrderItems = entity.OrderItems?.Select(orderItem => new OrderItemVM
                {
                    Quantity = orderItem.Quantity,
                    TotalPrice = orderItem.TotalPrice,
                    ProductID = orderItem.ProductID,
                    OrderID = orderItem.OrderID
                })
            };

            // Serialize the data using the configured settings

            if (entity is null)
            {
                return new Message
                {
                    IsValid = false,
                    Info = "Entitet nije pronađen",
                    Status = ExceptionCode.NotFound,
                };
            }

            if (orderVM is null)
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
        public async Task<Message> Create(OrderSnimiVM x)
        {
            Order? entity;

            var loggedUserId = (await AuthContext.GetLoggedUser()).Id;
            if (x.Id == 0)
            {
                entity = new Order();
                _dBContext.Add(entity);
            }
            else
            {
                entity = await _dBContext.Orders
                   
                    .FirstOrDefaultAsync(o => o.Id == x.Id);

                if (entity == null)
                {
                    return new Message
                    {
                        IsValid = false,
                        Info = "Order not found",
                        Status = ExceptionCode.NotFound,
                        Data = null
                    };
                }
            }
            // Map OrderItems from viewmodel to entity
            var orderItems = x.OrderItems?.Select(orderItemVm => new OrderItems
            {
                Quantity = orderItemVm.Quantity,
                TotalPrice = orderItemVm.TotalPrice,
                ProductID = orderItemVm.ProductID,
                OrderID=orderItemVm.OrderID
               
                
            }).ToList();

            entity.OrderItems = orderItems;
            entity.OrderDate = DateTime.Now;
            entity.UserID = loggedUserId;
            entity.Payment_intent_id = x.Payment_intent_id;
            entity.Receipt_url = x.Receipt_url;
            entity.isSubscribed = x.isSubscribed;
            entity.TotalTotalPrice = x.TotalTotalPrice;
            entity.Quantity = x.Quantity;
            entity.ShippingAdress = x.ShippingAdress;

            await _dBContext.SaveChangesAsync();

           

            var orderVM = new OrderVM
            {
                Id = entity.Id,
                OrderDate = entity.OrderDate,
                UserID = entity.UserID,
                Payment_intent_id = entity.Payment_intent_id,
                Receipt_url = entity.Receipt_url,
                isSubscribed = entity.isSubscribed,
                TotalTotalPrice = entity.TotalTotalPrice,
                Quantity = entity.Quantity,
                ShippingAdress = entity.ShippingAdress,
                OrderItems = orderItems?.Select(orderItem => new OrderItemVM
                {
                    Quantity = orderItem.Quantity,
                    TotalPrice = orderItem.TotalPrice,
                    ProductID = orderItem.ProductID,
                    OrderID =orderItem.OrderID
                })
            };
            return new Message
            {
                IsValid = true,
                Info = $"Entity {(x.Id == 0 ? "created" : "updated")}",
                Status = ExceptionCode.Success,
                Data = orderVM
            };
        }


        public async Task<Message> Delete(int id)
        {
            
            Order entity = await _dBContext.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == id);
            if (entity is null)
            {
                return new Message
                {
                    Status = ExceptionCode.NotFound,
                    Info = "Entitet nije pronadjen",
                    IsValid = false
                };
            }

            // Remove the reference to the Order entity in each OrderItem
            //foreach (var orderItem in entity.OrderItems)
            //{
            //    orderItem.Order = null;
            //}

            _dBContext.Orders.Remove(entity);
            await _dBContext.SaveChangesAsync();

            return new Message
            {
                Status = ExceptionCode.Success,
                Info = "Entitet uspjesno izbrisan",
                IsValid = true
            };
        }
    }
}
