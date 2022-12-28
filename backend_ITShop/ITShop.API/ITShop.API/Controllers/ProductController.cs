using ITShop.API.Helper;
using ITShop.API.Interface;
using ITShop.API.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Zaposlenik")]
    public class ProductController : ControllerBase
    {
        private IProductService _productService;

        public ProductController(IProductService _productService)
        {
            this._productService = _productService;
        }
        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] ProductGetVM x, int items_per_page = 10, int page_number = 1)
        {
            var message = await _productService.GetAllPaged(x, items_per_page, page_number);
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
        public async Task<ActionResult> Snimi([FromBody] ProductCreateVM x)
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
