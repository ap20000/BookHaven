using BookHaven.Services.Interface;
using System.Net.Mail;
using System.Net;

namespace BookHaven.Services
{
    public class SmtpEmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public SmtpEmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendAsync(string toEmail, string subject, string body)
        {
            var smtpHost = _config["Smtp:Host"];           // e.g., smtp.gmail.com
            var smtpPort = int.Parse(_config["Smtp:Port"]); // e.g., 587
            var smtpUser = _config["Smtp:User"];           // your email
            var smtpPass = _config["Smtp:Password"];       // app password

            var client = new SmtpClient(smtpHost, smtpPort)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(smtpUser, smtpPass)
            };

            var message = new MailMessage(smtpUser, toEmail, subject, body)
            {
                IsBodyHtml = false
            };

            await client.SendMailAsync(message);
        }
    }
}
