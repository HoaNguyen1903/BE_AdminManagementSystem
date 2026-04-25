using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
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

        public AuthService(IStaffRepository staffRepository, IUserRepository userRepository, IConfiguration configuration, IEmailService emailService)
        {
            _staffRepository = staffRepository;
            _userRepository = userRepository;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            var staff = await _staffRepository.GetByEmailAsync(request.Email);

            if (staff == null || staff.Password != request.Password)
            {
                return null;
            }

            var token = GenerateJwtToken(staff.StaffId, staff.Email, staff.Role);

            return new LoginResponse
            {
                Token = token,
                Email = staff.Email,
                Role = staff.Role,
                AvatarUrl = staff.AvatarUrl
            };
        }

        public async Task<LoginResponse?> LoginUserAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null || user.Password != request.Password)
            {
                return null;
            }

            if (!user.IsEmailVerified)
            {
                throw new InvalidOperationException("Email is not verified.");
            }

            user.LastOnline = DateTime.UtcNow;
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
                Password = request.Password,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                BannedUntil = null,
                LastOnline = DateTime.UtcNow,
                IsEmailVerified = false,
                EmailVerificationToken = verificationToken,
                EmailVerificationTokenExpiry = DateTime.UtcNow.AddHours(24)
            };

            var created = await _userRepository.AddAsync(user);

            // Send verification email
            var apiBaseUrl = _configuration["ApiBaseUrl"] ?? "https://localhost:7270";
            var verificationLink = $"{apiBaseUrl}/api/auth/verify-email?userId={created.UserId}&token={verificationToken}";
            await _emailService.SendVerificationEmailAsync(created.Email, created.FirstName, verificationLink);

            var token = GenerateJwtToken(created.UserId, created.Email, "User");

            return new LoginResponse
            {
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
            if (user == null || user.IsEmailVerified || user.EmailVerificationToken != token || (user.EmailVerificationTokenExpiry.HasValue && user.EmailVerificationTokenExpiry.Value < DateTime.UtcNow))
            {
                return false;
            }

            user.IsEmailVerified = true;
            user.EmailVerificationToken = null;
            user.EmailVerificationTokenExpiry = null;
            await _userRepository.UpdateAsync(user);

            return true;
        }

        private string GenerateJwtToken(int staffId, string email, string role)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, staffId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
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
