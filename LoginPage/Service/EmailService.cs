using Microsoft.AspNetCore.Identity.UI.Services;
using LoginPage.Interface;
using LoginPage.DTO;
using MimeKit;
using MailKit.Net.Smtp;
using MimeKit.Text;
using MailKit.Security;
namespace LoginPage.Service
{
    public class EmailService : IEmailService
    {

        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }
        public void SendEmailAsync(EmailDto request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("infodigital0909@gmail.com"));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) {Text = request.Body };

            var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("infodigital0909@gmail.com", "dxap qlgq wynp kfsz");
            smtp.Send(email);
            smtp.Disconnect(true);

        }
    }
}
