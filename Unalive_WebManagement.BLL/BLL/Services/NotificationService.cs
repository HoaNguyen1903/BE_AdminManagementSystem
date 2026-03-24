using Unalive_WebManagement.BLL.Interfaces;
using Unalive_WebManagement.DAL.Interfaces;
using Unalive_WebManagement.DTOs;

namespace Unalive_WebManagement.BLL.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<IEnumerable<NotificationDto>> GetAllNotificationsAsync()
        {
            var notifications = await _notificationRepository.GetAllAsync();
            return notifications.Select(n => new NotificationDto
            {
                NotificationId = n.NotificationId,
                NotificationMessage = n.NotificationMessage,
                ReceiverId = n.ReceiverId
            });
        }

        public async Task<NotificationDto?> GetNotificationByIdAsync(int id)
        {
            var notification = await _notificationRepository.GetByIdAsync(id);
            if (notification == null) return null;

            return new NotificationDto
            {
                NotificationId = notification.NotificationId,
                NotificationMessage = notification.NotificationMessage,
                ReceiverId = notification.ReceiverId
            };
        }
    }
}
