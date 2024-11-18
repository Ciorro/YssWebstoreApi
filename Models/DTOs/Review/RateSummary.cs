namespace YssWebstoreApi.Models.DTOs.Review
{
    public class RateSummary
    {
        public required int Rate { get; init; }
        public required int Count { get; init; }
        public required double Share { get; init; }
    }
}
