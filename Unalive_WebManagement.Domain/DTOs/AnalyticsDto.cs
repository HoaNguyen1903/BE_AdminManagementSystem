using System;

namespace Unalive_WebManagement.DTOs
{
    public class RevenueAnalyticsDto
    {
        public string Label { get; set; } = null!;
        public decimal Revenue { get; set; }
    }

    public class BundleRankingDto
    {
        public int BundleId { get; set; }
        public string BundleName { get; set; } = null!;
        public string Type { get; set; } = null!;
        public int Count { get; set; }
        public decimal Revenue { get; set; }
    }

    public class PlayerStatsDto
    {
        public int CurrentOnline { get; set; }
        public int DailyActiveUsers { get; set; }
        public int BannedAccountsCount { get; set; }
    }

    public class TopSpenderDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int TransactionCount { get; set; }
        public decimal TotalSpent { get; set; }
    }
}
