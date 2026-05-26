using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Resend;
using Unalive_WebManagement.BLL.Interfaces;

namespace Unalive_WebManagement.BLL.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;
        private readonly IResend _resend;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger, IResend resend)
        {
            _configuration = configuration;
            _logger = logger;
            _resend = resend;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var resendSettings = _configuration.GetSection("Resend");
            var fromEmail = resendSettings["FromEmail"] ?? "onboarding@resend.dev";
            var fromName = resendSettings["FromName"] ?? "Unalive Team";

            if (string.IsNullOrEmpty(to))
            {
                _logger.LogWarning("Email was not sent because 'to' address is missing.");
                return;
            }

            try
            {
                _logger.LogInformation("Attempting to send email to {To} via Resend API", to);
                
                var message = new EmailMessage();
                message.From = $"{fromName} <{fromEmail}>";
                message.To.Add(to);
                message.Subject = subject;
                message.HtmlBody = body;

                await _resend.EmailSendAsync(message);
                
                _logger.LogInformation("Email sent successfully to {To} via Resend", to);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email via Resend to {To}: {Message}", to, ex.Message);
                throw;
            }
        }

        public async Task SendVerificationEmailAsync(string to, string userName, string verificationLink)
        {
            var subject = "Unalive - Verify your email address";
            var body = $@"
                <div style='font-family: sans-serif;'>
                    <h1>Welcome to Unalive, {userName}!</h1>
                    <p>Thank you for registering. Please verify your email address by clicking the link below:</p>
                    <p><a href='{verificationLink}' style='background-color: #007bff; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px;'>Verify Email Address</a></p>
                    <p>If you did not register for an account, please ignore this email.</p>
                    <p>Best regards,<br/>The Unalive Team</p>
                </div>";

            await SendEmailAsync(to, subject, body);
        }

        public async Task SendTestEmailAsync(string to)
        {
            var subject = "Unalive - Resend Test Email";
            var body = $@"
                <div style='font-family: sans-serif;'>
                    <h1>Resend API Test</h1>
                    <p>This is a test email to verify that your Resend API configuration is working correctly on the server.</p>
                    <p>Sent at: {DateTime.UtcNow} UTC</p>
                    <p>Best regards,<br/>The Unalive Team</p>
                </div>";

            await SendEmailAsync(to, subject, body);
        }

        public async Task SendPasswordResetEmailAsync(string to, string userName, string resetLink)
        {
            var subject = "Unalive - Password Reset Request";
            var body = $@"
                <div style='font-family: sans-serif;'>
                    <h1>Password Reset Request</h1>
                    <p>Hello {userName},</p>
                    <p>We received a request to reset your password. Please click the button below to set a new password:</p>
                    <p><a href='{resetLink}' style='background-color: #007bff; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px;'>Reset Password</a></p>
                    <p>This link will expire in 1 hour. If you did not request a password reset, please ignore this email.</p>
                    <p>Best regards,<br/>The Unalive Team</p>
                </div>";

            await SendEmailAsync(to, subject, body);
        }
    }
}