using YssWebstoreApi.Models.DTOs.Accounts;

namespace YssWebstoreApi.Models.DTOs.Post
{
    public class PublicPost
    {
        public required ulong Id { get; set; }
        public required DateTimeOffset CreatedAt { get; set; }
        public required DateTimeOffset UpdatedAt { get; set; }
        public required PublicAccount Account { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public string? CoverUrl { get; set; }
    }
}
