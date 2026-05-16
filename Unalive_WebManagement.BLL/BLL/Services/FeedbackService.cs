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
        private readonly IUserRepository _userRepository;

        public FeedbackService(
            INotificationRepository notificationRepository, 
            IReportRepository reportRepository,
            IUserRepository userRepository)
        {
            _notificationRepository = notificationRepository;
            _reportRepository = reportRepository;
            _userRepository = userRepository;
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

        public async Task<IEnumerable<ReportDto>> GetAllReportsAsync(ReportFilterParameters query)
        {
            var items = await _reportRepository.GetAllAsync();
            var users = await _userRepository.GetAllAsync();
            var userDict = users.ToDictionary(u => u.UserId, u => u.UserName);

            var dtos = items.Select(r => new ReportDto
            {
                ReportId = r.ReportId,
                SenderId = r.SenderId,
                SenderUserName = userDict.ContainsKey(r.SenderId) ? userDict[r.SenderId] : "Unknown",
                AccusedId = r.AccusedId,
                AccusedUserName = userDict.ContainsKey(r.AccusedId) ? userDict[r.AccusedId] : "Unknown",
                Reason = r.Reason,
                AdditionalInfo = r.AdditionalInfo,
                SendDate = r.SendDate,
                Approved = r.Approved,
                ApprovedDate = r.ApprovedDate,
                ApprovedBy = r.ApprovedBy
            });

            var q = dtos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.SenderUserName))
            {
                q = q.Where(r => r.SenderUserName != null && r.SenderUserName.Contains(query.SenderUserName, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(query.AccusedUserName))
            {
                q = q.Where(r => r.AccusedUserName != null && r.AccusedUserName.Contains(query.AccusedUserName, StringComparison.OrdinalIgnoreCase));
            }

            if (query.StartDate.HasValue)
            {
                q = q.Where(r => r.SendDate >= query.StartDate.Value);
            }

            if (query.EndDate.HasValue)
            {
                q = q.Where(r => r.SendDate <= query.EndDate.Value);
            }

            if (query.Approved.HasValue)
            {
                q = q.Where(r => r.Approved == query.Approved.Value);
            }

            return q.ApplyQuery(query, (r, search) => 
                r.Reason.Contains(search, StringComparison.OrdinalIgnoreCase) || 
                (r.AdditionalInfo != null && r.AdditionalInfo.Contains(search, StringComparison.OrdinalIgnoreCase)) ||
                (r.SenderUserName != null && r.SenderUserName.Contains(search, StringComparison.OrdinalIgnoreCase)) ||
                (r.AccusedUserName != null && r.AccusedUserName.Contains(search, StringComparison.OrdinalIgnoreCase)));
        }

        public async Task<ReportDto?> GetReportByIdAsync(int id)
        {
            var r = await _reportRepository.GetByIdAsync(id);
            if (r == null) return null;

            var sender = await _userRepository.GetByIdAsync(r.SenderId);
            var accused = await _userRepository.GetByIdAsync(r.AccusedId);

            return new ReportDto
            {
                ReportId = r.ReportId,
                SenderId = r.SenderId,
                SenderUserName = sender?.UserName ?? "Unknown",
                AccusedId = r.AccusedId,
                AccusedUserName = accused?.UserName ?? "Unknown",
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
            
            var sender = await _userRepository.GetByIdAsync(senderId);
            var accused = await _userRepository.GetByIdAsync(dto.AccusedId);

            return new ReportDto
            {
                ReportId = created.ReportId,
                SenderId = created.SenderId,
                SenderUserName = sender?.UserName ?? "Unknown",
                AccusedId = created.AccusedId,
                AccusedUserName = accused?.UserName ?? "Unknown",
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
