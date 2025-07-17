using YssWebstoreApi.Api.DTO.Accounts;

namespace YssWebstoreApi.Api.DTO.Projects
{
    public class ProjectResponse
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime ReleasedAt { get; set; }
        public required AccountResponse Account { get; set; }
        public required string Name { get; set; }
        public required string Slug { get; set; }
        public required string Description { get; set; }
        public IList<string> Tags { get; set; } = [];
        public IList<string> Images { get; set; } = [];
    }
}
