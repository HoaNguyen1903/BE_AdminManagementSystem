using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unalive_WebManagement.BLL.Interfaces;
using Unalive_WebManagement.DTOs;

namespace Unalive_WebManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;

        public AuthController(IAuthService authService, IEmailService emailService)
        {
            _authService = authService;
            _emailService = emailService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            var response = await _authService.LoginAsync(request);
            if (response == null)
            {
                return Unauthorized("Invalid credentials");
            }
            return Ok(response);
        }

        [HttpPost("logout")]
        [Authorize(Roles = "Admin,Staff")]
        public IActionResult Logout()
        {
            // For staff, we just return OK as the token is cleared on the frontend.
            // We can add audit logging here later if needed.
            return Ok(new { message = "Logged out successfully" });
        }

        [HttpPost("login-user")]
        public async Task<ActionResult<LoginResponse>> LoginUser([FromBody] LoginRequest request)
        {
            try
            {
                var response = await _authService.LoginUserAsync(request);
                if (response == null)
                {
                    return Unauthorized("Invalid credentials");
                }
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<LoginResponse>> Register([FromBody] RegisterRequest request)
        {
            var response = await _authService.RegisterUserAsync(request);
            if (response == null)
            {
                return Conflict("Email already exists");
            }
            return Ok(response);
        }

        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromQuery] int userId, [FromQuery] string token)
        {
            var result = await _authService.VerifyEmailAsync(userId, token);
            if (!result)
            {
                return Redirect("/email-verification-failed.html");
            }
            return Redirect("/email-verified.html");
        }

        [HttpPost("resend-verification")]
        public async Task<IActionResult> ResendVerification([FromQuery] string email)
        {
            var result = await _authService.ResendVerificationEmailAsync(email);
            if (!result)
            {
                return BadRequest("User not found or already verified");
            }
            return Ok(new { message = "Verification email resent" });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            await _authService.ForgotPasswordAsync(request.Email, true);
            return Ok(new { message = "If the email exists, a reset link has been sent." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var result = await _authService.ResetPasswordAsync(request, true);
            if (!result) return BadRequest("Invalid or expired token.");
            return Ok(new { message = "Password reset successfully." });
        }

        [HttpPost("forgot-password-user")]
        public async Task<IActionResult> ForgotPasswordUser([FromBody] ForgotPasswordRequest request)
        {
            await _authService.ForgotPasswordAsync(request.Email, false);
            return Ok(new { message = "If the email exists, a reset link has been sent." });
        }

        [HttpPost("reset-password-user")]
        public async Task<IActionResult> ResetPasswordUser([FromBody] ResetPasswordRequest request)
        {
            var result = await _authService.ResetPasswordAsync(request, false);
            if (!result) return BadRequest("Invalid or expired token.");
            return Ok(new { message = "Password reset successfully." });
        }

        [HttpPost("test-email")]
        public async Task<IActionResult> TestEmail([FromQuery] string email)
        {
            try
            {
                await _emailService.SendTestEmailAsync(email);
                return Ok(new { message = "Test email sent successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to send test email", error = ex.Message });
            }
        }
    }
}
