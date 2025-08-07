using YssWebstoreApi.Api.DTO.Accounts;
using YssWebstoreApi.Api.DTO.Projects;

namespace YssWebstoreApi.Api.DTO.Posts
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
