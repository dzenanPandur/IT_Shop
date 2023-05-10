using ITShop.API.Database;
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
    [Authorize(Roles = "Zaposlenik")]
    public class ProductPictureController : ControllerBase
    {
        private IProductPictureService _productPictureService;
        public readonly ITShop_DBContext _dbContext;

        public ProductPictureController(IProductPictureService _productPictureService, ITShop_DBContext dbContext)
        {
            this._productPictureService = _productPictureService;
            _dbContext = dbContext;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetAll(int id)
        {
            var message = await _productPictureService.GetAll(id);
            if (!message.IsValid)
                return BadRequest(message);

            return Ok(message);
        }

        [HttpPost("{id}")]
        public async Task<ActionResult> AddProductPicture(int id, [FromForm] ProductPictureAddVM x)
        {
            var message = await _productPictureService.Create(id, x);
            if(message.IsValid)
                return Ok(message);
            
            return BadRequest(message.Info);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var message = await _productPictureService.Delete(id);
            if (!message.IsValid)
                return BadRequest(message);

            return Ok(message);
        }

     
    }
}
