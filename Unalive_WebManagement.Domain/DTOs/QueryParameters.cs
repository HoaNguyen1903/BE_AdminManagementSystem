namespace Unalive_WebManagement.DTOs
{
    public class QueryParameters
    {
        public string? Search { get; set; }
        public string? SortBy { get; set; }
        public bool IsDescending { get; set; } = false;
    }
}
