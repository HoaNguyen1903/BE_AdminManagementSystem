using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Unalive_WebManagement.BLL.Interfaces;
using Unalive_WebManagement.DAL.Interfaces;
using Unalive_WebManagement.DTOs;
using Unalive_WebManagement.Models;

namespace Unalive_WebManagement.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IStaffRepository _staffRepository;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly ILogger<AuthService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public AuthService(
            IStaffRepository staffRepository, 
            IUserRepository userRepository, 
            IConfiguration configuration, 
            IEmailService emailService,
            ILogger<AuthService> logger,
            IServiceScopeFactory scopeFactory)
        {
            _staffRepository = staffRepository;
            _userRepository = userRepository;
            _configuration = configuration;
            _emailService = emailService;
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            var staff = await _staffRepository.GetByEmailAsync(request.Email);
            if (staff == null) return null;

            bool isPasswordCorrect = false;
            bool needsUpgrade = false;

            try
            {
                isPasswordCorrect = BCrypt.Net.BCrypt.Verify(request.Password, staff.Password);
            }
            catch (BCrypt.Net.SaltParseException)
            {
                // Fallback for plaintext passwords during migration
                if (staff.Password == request.Password)
                {
                    isPasswordCorrect = true;
                    needsUpgrade = true;
                }
            }

            if (!isPasswordCorrect) return null;

            // Auto-migrate plaintext passwords to BCrypt hashes
            if (needsUpgrade)
            {
                staff.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
                await _staffRepository.UpdateAsync(staff);
                _logger.LogInformation("Automatically migrated password for staff {Email} to BCrypt hash.", staff.Email);
            }

            var token = GenerateJwtToken(staff.StaffId, staff.Email, staff.Role);

            return new LoginResponse
            {
                Id = staff.StaffId,
                Token = token,
                Email = staff.Email,
                UserName = staff.Email.Split('@')[0], // Use email prefix as UserName for staff
                Role = staff.Role,
                AvatarUrl = staff.AvatarUrl
            };
        }

        public async Task<LoginResponse?> LoginUserAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null) return null;

            bool isPasswordCorrect = false;
            bool needsUpgrade = false;

            try
            {
                isPasswordCorrect = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
            }
            catch (BCrypt.Net.SaltParseException)
            {
                // Fallback for plaintext passwords during migration
                if (user.Password == request.Password)
                {
                    isPasswordCorrect = true;
                    needsUpgrade = true;
                }
            }

            if (!isPasswordCorrect) return null;

            if (user.IsEmailVerified == 0)
            {
                throw new InvalidOperationException("Email is not verified.");
            }

            // Auto-migrate plaintext passwords to BCrypt hashes
            if (needsUpgrade)
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
                _logger.LogInformation("Automatically migrated password for user {Email} to BCrypt hash.", user.Email);
            }

            user.LastOnline = DateTime.UtcNow;
            user.IsOnline = 1;
            await _userRepository.UpdateAsync(user);

            // Default role for players is "User"
            var token = GenerateJwtToken(user.UserId, user.Email, "User");

            return new LoginResponse
            {
                Id = user.UserId,
                Token = token,
                Email = user.Email,
                UserName = user.UserName,
                Role = "User",
                AvatarUrl = user.AvatarUrl
            };
        }

        public async Task<LoginResponse?> RegisterUserAsync(RegisterRequest request)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return null;
            }

            var verificationToken = Guid.NewGuid().ToString();
            var user = new User
            {
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                BannedUntil = null,
                LastOnline = DateTime.UtcNow,
                IsEmailVerified = 0,
                EmailVerificationToken = verificationToken,
                EmailVerificationTokenExpiry = DateTime.UtcNow.AddHours(24)
            };

            var created = await _userRepository.AddAsync(user);

            // Send verification email in a background task with a fresh scope
            var apiBaseUrl = _configuration["ApiBaseUrl"] ?? "https://localhost:7270";
            var verificationLink = $"{apiBaseUrl}/api/auth/verify-email?userId={created.UserId}&token={verificationToken}";
            
            _ = Task.Run(async () =>
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedEmailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                    try
                    {
                        await scopedEmailService.SendVerificationEmailAsync(created.Email, created.FirstName, verificationLink);
                        _logger.LogInformation("Verification email sent to {Email}", created.Email);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to send verification email to {Email}", created.Email);
                    }
                }
            });

            var token = GenerateJwtToken(created.UserId, created.Email, "User");

            return new LoginResponse
            {
                Id = created.UserId,
                Token = token,
                Email = created.Email,
                UserName = created.UserName,
                Role = "User",
                AvatarUrl = created.AvatarUrl
            };
        }

        public async Task<bool> VerifyEmailAsync(int userId, string token)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null || user.IsEmailVerified == 1 || user.EmailVerificationToken != token || (user.EmailVerificationTokenExpiry.HasValue && user.EmailVerificationTokenExpiry.Value < DateTime.UtcNow))
            {
                return false;
            }

            user.IsEmailVerified = 1;
            user.EmailVerificationToken = null;
            user.EmailVerificationTokenExpiry = null;
            await _userRepository.UpdateAsync(user);

            return true;
        }

        public async Task<bool> ResendVerificationEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null || user.IsEmailVerified == 1)
            {
                return false;
            }

            var verificationToken = Guid.NewGuid().ToString();
            user.EmailVerificationToken = verificationToken;
            user.EmailVerificationTokenExpiry = DateTime.UtcNow.AddHours(24);
            await _userRepository.UpdateAsync(user);

            var apiBaseUrl = _configuration["ApiBaseUrl"] ?? "https://localhost:7270";
            var verificationLink = $"{apiBaseUrl}/api/auth/verify-email?userId={user.UserId}&token={verificationToken}";

            _ = Task.Run(async () =>
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedEmailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                    try
                    {
                        await scopedEmailService.SendVerificationEmailAsync(user.Email, user.FirstName, verificationLink);
                        _logger.LogInformation("Resent verification email to {Email}", user.Email);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to resend verification email to {Email}", user.Email);
                    }
                }
            });

            return true;
        }

        public async Task<bool> ForgotPasswordAsync(string email, bool isStaff)
        {
            string token = Guid.NewGuid().ToString();
            DateTimeOffset expiry = DateTimeOffset.UtcNow.AddHours(1);
            string userName = "";
            string resetLink = "";

            if (isStaff)
            {
                var staff = await _staffRepository.GetByEmailAsync(email);
                if (staff == null) return false;

                staff.PasswordResetToken = token;
                staff.PasswordResetTokenExpiry = expiry;
                await _staffRepository.UpdateAsync(staff);

                userName = staff.Email.Split('@')[0];
                var staffFrontendUrl = _configuration["StaffFrontendUrl"] ?? "https://admin.unalive.site";
                resetLink = $"{staffFrontendUrl}/reset-password?token={token}";
            }
            else
            {
                var user = await _userRepository.GetByEmailAsync(email);
                if (user == null) return false;

                user.PasswordResetToken = token;
                user.PasswordResetTokenExpiry = expiry;
                await _userRepository.UpdateAsync(user);

                userName = user.FirstName;
                var userFrontendUrl = _configuration["UserFrontendUrl"] ?? "https://unalive.site";
                resetLink = $"{userFrontendUrl}/reset-password?token={token}";
            }

            _ = Task.Run(async () =>
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scopedEmailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                    try
                    {
                        await scopedEmailService.SendPasswordResetEmailAsync(email, userName, resetLink);
                        _logger.LogInformation("Password reset email sent to {Email}", email);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to send password reset email to {Email}", email);
                    }
                }
            });

            return true;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordRequest request, bool isStaff)
        {
            if (isStaff)
            {
                var staff = await _staffRepository.GetByResetTokenAsync(request.Token);
                if (staff == null || !staff.PasswordResetTokenExpiry.HasValue || staff.PasswordResetTokenExpiry.Value < DateTimeOffset.UtcNow)
                {
                    return false;
                }

                staff.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
                staff.PasswordResetToken = null;
                staff.PasswordResetTokenExpiry = null;
                await _staffRepository.UpdateAsync(staff);
                return true;
            }
            else
            {
                var user = await _userRepository.GetByResetTokenAsync(request.Token);
                if (user == null || !user.PasswordResetTokenExpiry.HasValue || user.PasswordResetTokenExpiry.Value < DateTimeOffset.UtcNow)
                {
                    return false;
                }

                user.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
                user.PasswordResetToken = null;
                user.PasswordResetTokenExpiry = null;
                await _userRepository.UpdateAsync(user);
                return true;
            }
        }

        private string GenerateJwtToken(int staffId, string email, string role)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, staffId.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, staffId.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpiryInMinutes"]!)),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
