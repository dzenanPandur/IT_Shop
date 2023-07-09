using ITShop.API.Interface;
using ITShop.API.Services;
using ITShop.API.ViewModels.Role;
using ITShop.API.ViewModels.Subscription;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionController(ISubscriptionService _subscriptionService)
        {
            this._subscriptionService = _subscriptionService;
        }
        [HttpPost("create-subscription")]
        public async Task<IActionResult> CreateSubscriptionAsMessageAsync(SubscriptionCreateVM subscriptionCreateVM, CancellationToken cancellationToken)
        {
            var message = await _subscriptionService.CreateSubscriptionAsMessageAsync(subscriptionCreateVM, cancellationToken);
            if (message.IsValid == false)
                return BadRequest(message);
            return Ok(message);
        }

        //[HttpDelete("delete-Subscription")]
        //public async Task<IActionResult> DeleteSubscriptionAsMessageAsync(int id, CancellationToken cancellationToken)
        //{
        //    var message = await _subscriptionService.DeleteSubscriptionAsMessageAsync(id, cancellationToken);
        //    if (message.IsValid == false)
        //        return BadRequest(message);
        //    return Ok(message);
        //}

        [HttpGet("get-subscriptions")]
        public async Task<IActionResult> SubscriptionsGetAsMessageAsync(CancellationToken cancellationToken)
        {
            var message = await _subscriptionService.SubscriptionsGetAsMessageAsync(cancellationToken);
            if (message.IsValid == false)
                return BadRequest(message);
            return Ok(message);
        }

        [HttpGet("get-subscriptions-by-id")]
        [Authorize(Roles = "Kupac")]
        public async Task<IActionResult> SubscriptionsGetByIdAsMessageAsync(Guid id,CancellationToken cancellationToken)
        {
            var message = await _subscriptionService.SubscriptionsGetByIdAsMessageAsync(id, cancellationToken);
            if (message.IsValid == false)
                return BadRequest(message);
            return Ok(message);
        }

        [HttpPut("update-subscription")]
        public async Task<IActionResult> SubscriptionUpdateAsMessageAsync(SubscriptionUpdateVM subscriptionUpdateVM, CancellationToken cancellationToken)
        {
            var message = await _subscriptionService.SubscriptionUpdateAsMessageAsync(subscriptionUpdateVM, cancellationToken);
            if (message.IsValid == false)
                return BadRequest(message);
            return Ok(message);
        }
        [HttpGet("get-subscriptions-by-id-emp")]
        [Authorize(Roles = "Zaposlenik")]
        public async Task<IActionResult> SubscriptionsGetByIdEmpAsMessageAsync(Guid id, CancellationToken cancellationToken)
        {
            var message = await _subscriptionService.SubscriptionsGetByIdEmpAsMessageAsync(id, cancellationToken);
            if (message.IsValid == false)
                return BadRequest(message);
            return Ok(message);
        }
    }
}
