using ITShop.API.Entities;
using ITShop.API.Enums;
using ITShop.API.Helper;
using ITShop.API.ViewModels.Product;
using ITShop.API.ViewModels.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Management.ContainerInstance.Fluent.Models;
using Microsoft.EntityFrameworkCore;

namespace ITShop.API.Interface
{
    public interface IProductPictureService
    {
        Task<Message> GetAll(int id);
        Task<Message> Create(int id, ProductPictureAddVM vm);
        Task<Message> Delete(int id);
    }
}
