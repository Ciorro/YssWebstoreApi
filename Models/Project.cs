using YssWebstoreApi.Models.Interfaces;
using YssWebstoreApi.Models.Tags;

namespace YssWebstoreApi.Models
{
    public class Project : IEntity
    {
        public Guid Id { get; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime ReleasedAt { get; set; }
        public Guid AccountId { get; set; }
        public required string Name { get; set; }
        public required string Slug { get; set; }
        public required string Description { get; set; }
        public TagCollection Tags { get; set; } = [];
        public IList<Resource> Images { get; set; } = [];
        public bool IsPinned { get; set; }
    }
}
