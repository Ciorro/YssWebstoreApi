using YssWebstoreApi.Models.DTOs.Accounts;

namespace YssWebstoreApi.Models.DTOs.Review
{
    public class PublicReview
    {
        public required ulong Id { get; set; }
        public required DateTimeOffset CreatedAt { get; set; }
        public required DateTimeOffset UpdatedAt { get; set; }
        public required PublicAccount Account { get; set; }
        public required int Rate { get; set; }
        public string? Content { get; set; }
    }
}
