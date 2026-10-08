using System.ComponentModel.DataAnnotations;

namespace LoginPage.DTO
{
    public class RegisterDto
    {

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = "";
        [Required(ErrorMessage = "Passwor is required")]
        public string Password { get; set; } = "";
    }
  }
