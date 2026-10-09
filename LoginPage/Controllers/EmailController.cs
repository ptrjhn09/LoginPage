using LoginPage.DTO;
using LoginPage.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace LoginPage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost ("SendEmaail")]

        public IActionResult SendEmailAsync(EmailDto request)
        {
            _emailService.SendEmailAsync(request);

            return Ok(new
            {
                message = "You have sent new message!"
            });
        }

        [HttpPost ("ForgotPassowrd")]
        public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordDto forgot)
        {

            var result = await _emailService.ForgotPasswordAsync(forgot);

            if (result == null)
                return NotFound(new { message = "Email not found" });

            return Ok(new { token = result, message = "Reset token generated" });
        }
    
    }
}
