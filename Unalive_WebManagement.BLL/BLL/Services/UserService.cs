using Unalive_WebManagement.BLL.Interfaces;
using Unalive_WebManagement.DAL.Interfaces;
using Unalive_WebManagement.DTOs;
using Unalive_WebManagement.Models;

namespace Unalive_WebManagement.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(u => new UserDto
            {
                UserId = u.UserId,
                Email = u.Email,
                FullName = u.FullName,
                Banned = u.Banned
            });
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;

            return new UserDto
            {
                UserId = user.UserId,
                Email = user.Email,
                FullName = user.FullName,
                Banned = user.Banned
            };
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
        {
            // Note: Password hashing should be handled here or in a separate utility
            var user = new User
            {
                Email = dto.Email,
                Password = dto.Password, // WARNING: Plain text password. Should be hashed.
                FullName = dto.FullName,
                Banned = dto.Banned
            };

            var created = await _userRepository.AddAsync(user);

            return new UserDto
            {
                UserId = created.UserId,
                Email = created.Email,
                FullName = created.FullName,
                Banned = created.Banned
            };
        }

        public async Task UpdateUserAsync(int id, UpdateUserDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) throw new KeyNotFoundException($"User with ID {id} not found");

            user.Email = dto.Email;
            user.Password = dto.Password; // WARNING: Should be hashed
            user.FullName = dto.FullName;
            user.Banned = dto.Banned;

            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteUserAsync(int id)
        {
            await _userRepository.DeleteAsync(id);
        }
    }
}
