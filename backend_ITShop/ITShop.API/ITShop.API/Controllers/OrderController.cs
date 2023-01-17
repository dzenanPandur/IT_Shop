using ITShop.API.Interface;
using ITShop.API.Services;
using ITShop.API.ViewModels.Discount;
using ITShop.API.ViewModels.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Kupac")]
    public class OrderController: ControllerBase
    {
        private IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService; 
        }
        [HttpGet]
        public async Task<ActionResult> GetAll(int items_per_page = 10, int page_number = 1)
        {
            var message = await _orderService.GetAllPaged(items_per_page, page_number);
            if (!message.IsValid)
                return BadRequest(message);

            return Ok(message);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var message = await _orderService.Get(id);
            if (!message.IsValid)
                return BadRequest(message);

            return Ok(message);
        }
        [HttpPost]
        public async Task<ActionResult> Snimi([FromBody] OrderSnimiVM x)
        {
            var message = await _orderService.Snimi(x);
            if (!message.IsValid)
                return BadRequest(message);

            return Ok(message);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var message = await _orderService.Delete(id);
            if (!message.IsValid)
                return BadRequest(message);

            return Ok(message);
        }
    }
}
