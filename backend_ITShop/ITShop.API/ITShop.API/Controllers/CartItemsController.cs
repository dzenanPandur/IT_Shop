using ITShop.API.Interface;
using ITShop.API.Services;
using ITShop.API.ViewModels.CartItems;
using ITShop.API.ViewModels.Discount;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Zaposlenik")]
    public class CartItemsController: ControllerBase
    {
        private ICartItemsService _cartItemsService;
        public CartItemsController(ICartItemsService cartItemsService)
        {
            _cartItemsService = cartItemsService;
        }
        [HttpGet]
        public async Task<ActionResult> GetAll(int items_per_page = 10, int page_number = 1)
        {
            var message = await _cartItemsService.GetAllPaged(items_per_page, page_number);
            if (!message.IsValid)
                return BadRequest(message);

            return Ok(message);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var message = await _cartItemsService.Get(id);
            if (!message.IsValid)
                return BadRequest(message);

            return Ok(message);
        }
        [HttpPost]
        public async Task<ActionResult> Snimi([FromBody] CartItemsSnimiVM x)
        {
            var message = await _cartItemsService.Snimi(x);
            if (!message.IsValid)
                return BadRequest(message);

            return Ok(message);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var message = await _cartItemsService.Delete(id);
            if (!message.IsValid)
                return BadRequest(message);

            return Ok(message);
        }
    }
}
