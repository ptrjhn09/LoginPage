using Microsoft.AspNetCore.Mvc;
using LoginPage.Data;
using LoginPage.DTO;
using LoginPage.Interface;
using LoginPage.Service;
using Microsoft.AspNetCore.Authorization;

namespace LoginPage.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authservice;

        public AuthController(IAuthService authservice)
        {
            _authservice = authservice;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto request)
        {
            var result = await _authservice.LoginAsync(request);
             
            if(result == null)
            {
                return Unauthorized(new {message = "Invalid Email or Password" });
            }

            if(result == "LOCKED")
            {
                return BadRequest(new { messeage = "Your account is blocked please try again later." });
            }
            return Ok(result);
        }

        [Authorize]
        [HttpGet("profile")]
        public IActionResult Profile()
        {
            return Ok(new
            {
                message = "You are authenticated!"
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto request)
        {
            var result = await _authservice.RegisterAsync(request);

            if (result == null)
            {
                return BadRequest(new { message = "Email already Registered" });
            }

            return Ok(new
            {
                message = "Successfully Registered",
            });
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordDto forgot)
        {
           
            var result = await _authservice.ForgotPasswordAsync(forgot);

            if (result == null)
            {
                return BadRequest(new { message = "Email not found" });
            }
            

            return Ok(new {message="Reset Email link send Succesfully"});
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto reset)
        {
            var result = await _authservice.ResetPassword(reset);

            if(result != "Password Reset Succesfuly")
            {
                return BadRequest(new { message = result });
            }

            return Ok(new { message = result });

        }
    }
}
