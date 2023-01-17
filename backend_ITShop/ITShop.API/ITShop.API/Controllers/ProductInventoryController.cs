using ITShop.API.Interface;
using ITShop.API.ViewModels.ProductCategory;
using ITShop.API.ViewModels.ProductInventory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Zaposlenik")]
    public class ProductInventoryController: ControllerBase
    {
        private IProductInventoryService _productInventoryService;
        public ProductInventoryController(IProductInventoryService productInventoryService)
        {
            _productInventoryService = productInventoryService;
        }
        [HttpGet]
        public async Task<ActionResult> GetAll(int items_per_page = 10, int page_number = 1)
        {
            var message = await _productInventoryService.GetAllPaged(items_per_page, page_number);
            if (!message.IsValid)
                return BadRequest(message);

            return Ok(message);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var message = await _productInventoryService.Get(id);
            if (!message.IsValid)
                return BadRequest(message);

            return Ok(message);
        }
        [HttpPost]
        public async Task<ActionResult> Snimi([FromBody] ProductInventorySnimiVM x)
        {
            var message = await _productInventoryService.Snimi(x);
            if (!message.IsValid)
                return BadRequest(message);

            return Ok(message);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var message = await _productInventoryService.Delete(id);
            if (!message.IsValid)
                return BadRequest(message);

            return Ok(message);
        }
    }
}
