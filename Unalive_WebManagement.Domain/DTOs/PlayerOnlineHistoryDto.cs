using System;

namespace Unalive_WebManagement.DTOs
{
    public class PlayerOnlineHistoryDto
    {
        public DateTime Date { get; set; }
        public int OnlineCount { get; set; }
        public int DailyActiveUsers { get; set; }
    }
}
