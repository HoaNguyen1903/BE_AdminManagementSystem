using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Unalive_WebManagement.BLL.Interfaces;

namespace Unalive_WebManagement.BLL.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
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
                _logger.LogWarning("Email to {To} with subject '{Subject}' was not sent because SMTP is not configured.", to, subject);
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
                    // For debugging and handling potential SSL issues in cloud environments
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    client.Timeout = 30000; // 30 seconds timeout

                    // Use StartTls for port 587, SslOnConnect for 465
                    var socketOptions = port == 465 ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls;
                    if (!enableSsl) socketOptions = SecureSocketOptions.None;

                    _logger.LogInformation("Attempting to connect to SMTP host {Host}:{Port} with {Options}", host, port, socketOptions);
                    await client.ConnectAsync(host, port, socketOptions);
                    
                    _logger.LogInformation("Attempting to authenticate user {UserName}", userName);
                    await client.AuthenticateAsync(userName, password);
                    
                    _logger.LogInformation("Sending email to {To}", to);
                    await client.SendAsync(message);
                    
                    _logger.LogInformation("Email sent successfully to {To}", to);
                    await client.DisconnectAsync(true);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error sending email via MailKit to {To}: {Message}", to, ex.Message);
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

        public async Task SendTestEmailAsync(string to)
        {
            var subject = "Unalive - Test Email";
            var body = $@"
                <h1>SMTP Configuration Test</h1>
                <p>This is a test email to verify that your SMTP settings are working correctly on the server.</p>
                <p>Sent at: {DateTime.UtcNow} UTC</p>
                <p>Best regards,<br/>The Unalive Team</p>";

            await SendEmailAsync(to, subject, body);
        }
    }
}