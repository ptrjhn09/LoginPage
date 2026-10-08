using LoginPage.DTO;
using LoginPage.Interface;
using Microsoft.AspNetCore.Http;
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

        [HttpPost]
        
        public IActionResult SendEmailAsync(EmailDto request)
        {
            _emailService.SendEmailAsync(request);

            return Ok(new
            {
                message = "You have sent new message!"
            });
        }
    }
}
