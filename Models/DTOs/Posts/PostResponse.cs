using YssWebstoreApi.Models.DTOs.Accounts;
using YssWebstoreApi.Models.DTOs.Projects;

namespace YssWebstoreApi.Models.DTOs.Posts
{
    public class PostResponse
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public required AccountResponse Account { get; set; }
        public ProjectLinkResponse? Project { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public string? CoverImageUrl { get; set; }
    }
}
