using LoginPage.Data;
using LoginPage.DTO;
using LoginPage.Interface;
using LoginPage.Model;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using MimeKit.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace LoginPage.Service
{
    public class EmailService : IEmailService
    {

        private readonly IConfiguration _config;
        private readonly AppDbContext _context;

        public EmailService(IConfiguration config, AppDbContext context)
        {
            _config = config;
            _context = context;
        }

        public async Task<string> GeneratePasswordResetToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim("purpose", "reset-password"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["JWT:Key"]!)
                );

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
                (
                issuer: _config["JWT:PasswordResetIssuer"],
                audience: _config["JWT:PasswordResetAudience"],
                claims:claims,
                expires:DateTime.UtcNow.AddMinutes(10),
                signingCredentials:credentials

                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        public void SendEmailAsync(EmailDto request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("infodigital0909@gmail.com"));
            email.To.Add(MailboxAddress.Parse(request.To)); 
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

            var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("infodigital0909@gmail.com", "dxap qlgq wynp kfsz");
            smtp.Send(email);
            smtp.Disconnect(true);

        }

        public async Task<string?> ForgotPasswordAsync(ForgotPasswordDto forgot)
        {
            var user = await _context.Users.FirstOrDefaultAsync(e => e.Email == forgot.Email);
            if (user == null)
            {
                return null;
            }

            var token = await GeneratePasswordResetToken(user);

            user.PasswordResetToken = token;
            user.PasswordResetTokenExpiry = DateTime.UtcNow.AddMinutes(10);
            await _context.SaveChangesAsync();

            return token;
        }


    }
}
      