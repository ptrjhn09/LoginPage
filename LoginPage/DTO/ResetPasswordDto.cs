namespace LoginPage.DTO
{
    public class ResetPasswordDto
    {
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public string ResetToken { get; set; }
    }
}
