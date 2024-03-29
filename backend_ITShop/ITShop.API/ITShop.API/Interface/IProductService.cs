﻿using ITShop.API.Entities;
using ITShop.API.Enums;
using ITShop.API.Helper;

using ITShop.API.ViewModels.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Management.ContainerInstance.Fluent.Models;
using Microsoft.EntityFrameworkCore;

namespace ITShop.API.Interface
{
    public interface IProductService
    {
        Task<Message> GetAllPaged(ProductGetVM vm, int items_per_page = 10, int page_number = 1);
        Task<Message> GetMinMaxPrices(ProductGetVM vm);

        Task<Message> Get(int id);

        Task<Message> Snimi(ProductSnimiVM x);

        Task<Message> Delete(int id);
        
    }
}
