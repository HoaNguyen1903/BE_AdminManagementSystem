using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unalive_WebManagement.Domain.DTOs
{

    public class LeaderboardResponse
    {
        public string Message { get; set; } = "Success";
        public LeaderboardMeta Meta { get; set; }
        public LeaderboardData Data { get; set; }
    }
    public class LeaderboardMeta
    {
        public string Direction { get; set; }
        public bool HasBefore { get; set; }
        public bool HasAfter { get; set; }
        public long TopRank { get; set; }
        public long BottomRank { get; set; }
    }

    public class PlayerRankDto
    {
        public long Rank { get; set; }
        public int UserId { get; set; }
        public string DisplayName { get; set; }
        public string AvatarUrl { get; set; }
        public long RankPoint { get; set; }
    }

    public class LeaderboardData
    {
        public List<PlayerRankDto> Rankings { get; set; }
    }
}
