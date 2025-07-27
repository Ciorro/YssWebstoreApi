using YssWebstoreApi.Api.DTO.Accounts;

namespace YssWebstoreApi.Api.DTO.Reviews
{
    public class ReviewResponse
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public required AccountResponse Account { get; set; }
        public required int Rate { get; set; }
        public string? Content { get; set; }
    }
}
