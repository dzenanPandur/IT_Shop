using ITShop.API.Interface;
using ITShop.API.Services;
using ITShop.API.ViewModels.Discount;
using ITShop.API.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Zaposlenik")]
    public class DiscountController: ControllerBase
    {
        private IDiscountService _discountService;
        public DiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }
        [HttpGet]
        public async Task<ActionResult> GetAll(int items_per_page = 10, int page_number = 1)
        {
            var message = await _discountService.GetAllPaged(items_per_page, page_number);
            if (!message.IsValid)
                return BadRequest(message);

            return Ok(message);
        }
        [HttpGet("{id}"), AllowAnonymous]
        public async Task<ActionResult> Get(int id)
        {
            var message = await _discountService.Get(id);
            if (!message.IsValid)
                return BadRequest(message);

            return Ok(message);
        }
        [HttpPost]
        public async Task<ActionResult> Snimi([FromBody] DiscountSnimiVM x)
        {
            var message = await _discountService.Snimi(x);
            if (!message.IsValid)
                return BadRequest(message);

            return Ok(message);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var message = await _discountService.Delete(id);
            if (!message.IsValid)
                return BadRequest(message);

            return Ok(message);
        }
    }
}
