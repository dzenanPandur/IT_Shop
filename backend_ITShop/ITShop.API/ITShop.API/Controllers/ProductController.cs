﻿using ITShop.API.Database;
using ITShop.API.Entities;
using ITShop.API.Helper;
using ITShop.API.Interface;

using ITShop.API.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Management.Network.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics.Tracing;
using Index = Microsoft.EntityFrameworkCore.Metadata.Internal.Index;
using ITShop.API.ViewModels.Product;

namespace ITShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Zaposlenik")]
    public class ProductController : ControllerBase
    {
        private IProductService _productService;
        public readonly ITShop_DBContext _dbContext;

        public ProductController(IProductService _productService,ITShop_DBContext dbContext)
        {
            this._productService = _productService;
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] ProductGetVM x, int items_per_page = 10, int page_number = 1)
        {
            var message = await _productService.GetAllPaged(x, items_per_page, page_number);
            if (!message.IsValid)
                return BadRequest(message);

            return Ok(message);
        }
        
        [HttpGet("GetMinMaxPrices")]
        public async Task<ActionResult> GetMinMaxPrices([FromQuery] ProductGetVM x)
        {
            var message = await _productService.GetMinMaxPrices(x);
            if (!message.IsValid)
                return BadRequest(message);

            return Ok(message);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var message = await _productService.Get(id);
            if (!message.IsValid)
                return BadRequest(message);

            return Ok(message);
        }

        [HttpPost]
        public async Task<ActionResult> Snimi([FromBody] ProductSnimiVM x)
        {
            var message = await _productService.Snimi(x);
            if (!message.IsValid)
                return BadRequest(message);

            return Ok(message);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var message = await _productService.Delete(id);
            if (!message.IsValid)
                return BadRequest(message);

            return Ok(message);
        }

    }
}
