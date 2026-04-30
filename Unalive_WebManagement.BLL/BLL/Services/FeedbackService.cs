using Unalive_WebManagement.BLL.Interfaces;
using Unalive_WebManagement.DAL.Interfaces;
using Unalive_WebManagement.DTOs;
using Unalive_WebManagement.Models;
using Unalive_WebManagement.BLL.Helpers;

namespace Unalive_WebManagement.BLL.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IReportRepository _reportRepository;

        public FeedbackService(INotificationRepository notificationRepository, IReportRepository reportRepository)
        {
            _notificationRepository = notificationRepository;
            _reportRepository = reportRepository;
        }

        public async Task<IEnumerable<NotificationDto>> GetAllNotificationsAsync(QueryParameters query)
        {
            var items = await _notificationRepository.GetAllAsync();
            var dtos = items.Select(n => new NotificationDto
            {
                NotificationId = n.NotificationId,
                NotificationMessage = n.NotificationMessage,
                ReceiverId = n.ReceiverId,
                Read = n.Read
            });
            return dtos.ApplyQuery(query, (n, search) => n.NotificationMessage.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<IEnumerable<NotificationDto>> GetNotificationsByUserIdAsync(int userId, QueryParameters query)
        {
            var items = await _notificationRepository.GetAllAsync();
            var dtos = items.Where(n => n.ReceiverId == userId).Select(n => new NotificationDto
            {
                NotificationId = n.NotificationId,
                NotificationMessage = n.NotificationMessage,
                ReceiverId = n.ReceiverId,
                Read = n.Read
            });
            return dtos.ApplyQuery(query, (n, search) => n.NotificationMessage.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        public async Task MarkNotificationReadAsync(int id)
        {
            var n = await _notificationRepository.GetByIdAsync(id);
            if (n == null) throw new KeyNotFoundException();
            n.Read = 1;
            await _notificationRepository.UpdateAsync(n);
        }

        public async Task MarkNotificationUnreadAsync(int id)
        {
            var n = await _notificationRepository.GetByIdAsync(id);
            if (n == null) throw new KeyNotFoundException();
            n.Read = 0;
            await _notificationRepository.UpdateAsync(n);
        }

        public async Task<IEnumerable<ReportDto>> GetAllReportsAsync(QueryParameters query)
        {
            var items = await _reportRepository.GetAllAsync();
            var dtos = items.Select(r => new ReportDto
            {
                ReportId = r.ReportId,
                SenderId = r.SenderId,
                AccusedId = r.AccusedId,
                Reason = r.Reason,
                AdditionalInfo = r.AdditionalInfo,
                SendDate = r.SendDate,
                Approved = r.Approved,
                ApprovedDate = r.ApprovedDate,
                ApprovedBy = r.ApprovedBy
            });
            return dtos.ApplyQuery(query, (r, search) => r.Reason.Contains(search, StringComparison.OrdinalIgnoreCase) || (r.AdditionalInfo != null && r.AdditionalInfo.Contains(search, StringComparison.OrdinalIgnoreCase)));
        }

        public async Task<ReportDto?> GetReportByIdAsync(int id)
        {
            var r = await _reportRepository.GetByIdAsync(id);
            if (r == null) return null;
            return new ReportDto
            {
                ReportId = r.ReportId,
                SenderId = r.SenderId,
                AccusedId = r.AccusedId,
                Reason = r.Reason,
                AdditionalInfo = r.AdditionalInfo,
                SendDate = r.SendDate,
                Approved = r.Approved,
                ApprovedDate = r.ApprovedDate,
                ApprovedBy = r.ApprovedBy
            };
        }

        public async Task<ReportDto> CreateReportAsync(CreateReportDto dto, int senderId)
        {
            var r = new Report
            {
                SenderId = senderId,
                AccusedId = dto.AccusedId,
                Reason = dto.Reason,
                AdditionalInfo = dto.AdditionalInfo,
                SendDate = DateTime.UtcNow,
                Approved = 0
            };
            var created = await _reportRepository.AddAsync(r);
            return new ReportDto
            {
                ReportId = created.ReportId,
                SenderId = created.SenderId,
                AccusedId = created.AccusedId,
                Reason = created.Reason,
                AdditionalInfo = created.AdditionalInfo,
                SendDate = created.SendDate,
                Approved = created.Approved
            };
        }

        public async Task ApproveReportAsync(int id, int staffId)
        {
            var r = await _reportRepository.GetByIdAsync(id);
            if (r == null) throw new KeyNotFoundException();
            r.Approved = 1;
            r.ApprovedDate = DateTime.UtcNow;
            r.ApprovedBy = staffId;
            await _reportRepository.UpdateAsync(r);
        }
    }
}
