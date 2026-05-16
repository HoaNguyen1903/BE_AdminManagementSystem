using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
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
            var fromName = smtpSettings["FromName"] ?? "Unalive Team";

            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(userName))
            {
                // If SMTP is not configured, just log it for now
                Console.WriteLine($"Email to {to} with subject '{subject}' was not sent because SMTP is not configured.");
                return;
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(fromName, fromEmail ?? userName));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = body
            };
            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                try
                {
                    // Use StartTls for port 587, SslOnConnect for 465
                    var socketOptions = port == 465 ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls;
                    if (!enableSsl) socketOptions = SecureSocketOptions.None;

                    await client.ConnectAsync(host, port, socketOptions);
                    await client.AuthenticateAsync(userName, password);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
                catch (Exception ex)
                {
                    // Log the error
                    Console.WriteLine($"Error sending email via MailKit: {ex.Message}");
                    throw;
                }
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