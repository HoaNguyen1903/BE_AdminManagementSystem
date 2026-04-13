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

        public AuthService(IStaffRepository staffRepository, IUserRepository userRepository, IConfiguration configuration)
        {
            _staffRepository = staffRepository;
            _userRepository = userRepository;
            _configuration = configuration;
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
                Role = staff.Role
            };
        }

        public async Task<LoginResponse?> LoginUserAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null || user.Password != request.Password)
            {
                return null;
            }

            // Default role for players is "User"
            var token = GenerateJwtToken(user.UserId, user.Email, "User");

            return new LoginResponse
            {
                Token = token,
                Email = user.Email,
                Role = "User"
            };
        }

        public async Task<LoginResponse?> RegisterUserAsync(RegisterRequest request)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return null;
            }

            var user = new User
            {
                Email = request.Email,
                Password = request.Password,
                FirstName = request.FirstName,
                LastName = request.LastName,
                BannedUntil = null
            };

            var created = await _userRepository.AddAsync(user);

            var token = GenerateJwtToken(created.UserId, created.Email, "User");

            return new LoginResponse
            {
                Token = token,
                Email = created.Email,
                Role = "User"
            };
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
