using ITShop.API.Interface;
using ITShop.API.ViewModels.Discount;
using ITShop.API.ViewModels.Review;
using Microsoft.AspNetCore.Mvc;

namespace ITShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController:ControllerBase
    {
        private IReviewsService _reviewService;
        public ReviewController(IReviewsService reviewService)
        {
            _reviewService = reviewService;
        }
        [HttpPost]
        public async Task<ActionResult> Snimi([FromBody] ReviewSnimiVM x)
        {
            var message = await _reviewService.Snimi(x);
            if (!message.IsValid)
                return BadRequest(message);

            return Ok(message);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var message = await _reviewService.Delete(id);
            if (!message.IsValid)
                return BadRequest(message);

            return Ok(message);
        }
    }
}
