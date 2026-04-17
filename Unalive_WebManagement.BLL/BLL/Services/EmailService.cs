using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using Unalive_WebManagement.BLL.Interfaces;

namespace Unalive_WebManagement.BLL.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var smtpSettings = _configuration.GetSection("Smtp");
            var host = smtpSettings["Host"];
            var port = int.Parse(smtpSettings["Port"] ?? "587");
            var enableSsl = bool.Parse(smtpSettings["EnableSsl"] ?? "true");
            var userName = smtpSettings["UserName"];
            var password = smtpSettings["Password"];
            var fromEmail = smtpSettings["FromEmail"];

            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(userName))
            {
                // If SMTP is not configured, just log it for now
                Console.WriteLine($"Email to {to} with subject '{subject}' was not sent because SMTP is not configured.");
                return;
            }

            using (var client = new SmtpClient(host, port))
            {
                client.EnableSsl = enableSsl;
                client.Credentials = new NetworkCredential(userName, password);

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail ?? userName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(to);

                await client.SendMailAsync(mailMessage);
            }
        }

        public async Task SendVerificationEmailAsync(string to, string userName, string verificationLink)
        {
            var subject = "Unalive - Verify your email address";
            var body = $@"
                <h1>Welcome to Unalive, {userName}!</h1>
                <p>Thank you for registering. Please verify your email address by clicking the link below:</p>
                <p><a href='{verificationLink}'>Verify Email Address</a></p>
                <p>If you did not register for an account, please ignore this email.</p>
                <p>Best regards,<br/>The Unalive Team</p>";

            await SendEmailAsync(to, subject, body);
        }
    }
}