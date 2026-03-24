namespace Unalive_WebManagement.DTOs
{
    public class NotificationDto
    {
        public int NotificationId { get; set; }
        public string NotificationMessage { get; set; } = null!;
        public int ReceiverId { get; set; }
    }
}
