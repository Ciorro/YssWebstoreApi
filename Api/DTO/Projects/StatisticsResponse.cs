namespace YssWebstoreApi.Api.DTO.Projects
{
    public class StatisticsResponse
    {
        public required DateOnly RangeStart { get; set; }
        public required DateOnly RangeEnd { get; set; }
        public Dictionary<DateOnly, int> Downloads { get; set; } = [];
    }
}
