using ITShop.API.Interface;
using ITShop.API.ViewModels.Order;
using ITShop.API.ViewModels.OrderDetails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Zaposlenik")]
    public class OrderDetailsController: ControllerBase
    {
        private IOrderDetailsService _orderDetailsService;
        public OrderDetailsController(IOrderDetailsService orderDetailsService)
        {
            _orderDetailsService = orderDetailsService;
        }
        [HttpGet]
        public async Task<ActionResult> GetAll(int items_per_page = 10, int page_number = 1)
        {
            var message = await _orderDetailsService.GetAllPaged(items_per_page, page_number);
            if (!message.IsValid)
                return BadRequest(message);

            return Ok(message);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var message = await _orderDetailsService.Get(id);
            if (!message.IsValid)
                return BadRequest(message);

            return Ok(message);
        }
        [HttpPost]
        public async Task<ActionResult> Snimi([FromBody] OrderDetailsSnimiVM x)
        {
            var message = await _orderDetailsService.Snimi(x);
            if (!message.IsValid)
                return BadRequest(message);

            return Ok(message);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var message = await _orderDetailsService.Delete(id);
            if (!message.IsValid)
                return BadRequest(message);

            return Ok(message);
        }
    }
}
