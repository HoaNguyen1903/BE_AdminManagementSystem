using System;

namespace Unalive_WebManagement.DTOs
{
    public class NotificationDto
    {
        public int NotificationId { get; set; }
        public string NotificationMessage { get; set; } = null!;
        public int ReceiverId { get; set; }
        public bool Read { get; set; }
    }

    public class ReportDto
    {
        public int ReportId { get; set; }
        public int SenderId { get; set; }
        public int AccusedId { get; set; }
        public string Reason { get; set; } = null!;
        public string? AdditionalInfo { get; set; }
        public DateTime SendDate { get; set; }
        public bool Approved { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public int? ApprovedBy { get; set; }
    }

    public class CreateReportDto
    {
        public int AccusedId { get; set; }
        public string Reason { get; set; } = null!;
        public string? AdditionalInfo { get; set; }
    }
}
