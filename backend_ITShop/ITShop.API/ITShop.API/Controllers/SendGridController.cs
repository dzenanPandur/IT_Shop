using ITShop.API.Interface;
using ITShop.API.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class SendGridController : ControllerBase
    {

        private ISendGridService _sendgridService;

        public SendGridController(ISendGridService _sendgridService)
        {
            this._sendgridService = _sendgridService;
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> SendEnquiryEmail(string name, string email, string message, string subject)
        {
            var _message = await _sendgridService.SendEnquiryEmail(name, email, message, subject);
            return _message
                ? Ok()
                : new StatusCodeResult(500);
        }

        [HttpPost("send-email-troubleshoot"), Authorize(Roles ="Kupac")]
        public async Task<IActionResult> SendEmailTroubleshoot(string title, string message, string description)
        {
            var _message = await _sendgridService.SendEmailTroubleshoot(title, message, description);
            return _message
                ? Ok()
                : new StatusCodeResult(500);
        }
    }
}
