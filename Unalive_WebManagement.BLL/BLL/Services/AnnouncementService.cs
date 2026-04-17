using Unalive_WebManagement.BLL.Interfaces;
using Unalive_WebManagement.DAL.Interfaces;
using Unalive_WebManagement.DTOs;
using Unalive_WebManagement.Models;
using Unalive_WebManagement.BLL.Helpers;

namespace Unalive_WebManagement.BLL.Services
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly IAnnouncementRepository _announcementRepository;

        public AnnouncementService(IAnnouncementRepository announcementRepository)
        {
            _announcementRepository = announcementRepository;
        }

        public async Task<IEnumerable<AnnouncementDto>> GetAllAnnouncementsAsync(QueryParameters query)
        {
            var items = await _announcementRepository.GetAllAsync();
            var dtos = items.Select(a => new AnnouncementDto
            {
                AnnouncementId = a.AnnouncementId,
                Title = a.Title,
                Content = a.Content,
                Type = a.Type,
                Status = a.Status,
                StartDate = a.StartDate,
                EndDate = a.EndDate,
                CreatedBy = a.CreatedBy,
                CreatedAt = a.CreatedAt,
                UpdatedBy = a.UpdatedBy,
                UpdatedAt = a.UpdatedAt
            });
            return dtos.ApplyQuery(query, (a, search) => a.Title.Contains(search, StringComparison.OrdinalIgnoreCase) || a.Content.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<AnnouncementDto?> GetAnnouncementByIdAsync(int id)
        {
            var a = await _announcementRepository.GetByIdAsync(id);
            if (a == null) return null;
            return new AnnouncementDto
            {
                AnnouncementId = a.AnnouncementId,
                Title = a.Title,
                Content = a.Content,
                Type = a.Type,
                Status = a.Status,
                StartDate = a.StartDate,
                EndDate = a.EndDate,
                CreatedBy = a.CreatedBy,
                CreatedAt = a.CreatedAt,
                UpdatedBy = a.UpdatedBy,
                UpdatedAt = a.UpdatedAt
            };
        }

        public async Task<AnnouncementDto> CreateAnnouncementAsync(CreateAnnouncementDto dto, int staffId)
        {
            var a = new Announcement
            {
                Title = dto.Title,
                Content = dto.Content,
                Type = dto.Type,
                Status = dto.Status,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                CreatedBy = staffId,
                CreatedAt = DateTime.UtcNow
            };
            var created = await _announcementRepository.AddAsync(a);
            return await GetAnnouncementByIdAsync(created.AnnouncementId) ?? throw new Exception("Failed to create announcement");
        }

        public async Task<AnnouncementDto> UpdateAnnouncementAsync(int id, UpdateAnnouncementDto dto, int staffId)
        {
            var a = await _announcementRepository.GetByIdAsync(id);
            if (a == null) throw new KeyNotFoundException();
            a.Title = dto.Title;
            a.Content = dto.Content;
            a.Type = dto.Type;
            a.Status = dto.Status;
            a.StartDate = dto.StartDate;
            a.EndDate = dto.EndDate;
            a.UpdatedBy = staffId;
            a.UpdatedAt = DateTime.UtcNow;
            await _announcementRepository.UpdateAsync(a);
            return await GetAnnouncementByIdAsync(id) ?? throw new Exception("Failed to get updated announcement");
        }

        public async Task DeleteAnnouncementAsync(int id) => await _announcementRepository.DeleteAsync(id);
    }
}
