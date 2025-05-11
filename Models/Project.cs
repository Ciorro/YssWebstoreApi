using YssWebstoreApi.Models.Interfaces;

namespace YssWebstoreApi.Models
{
    public class Project : IEntity
    {
        public Guid Id { get; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid AccountId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public IList<Tag> Tags { get; set; } = [];
        public IList<Image> Images { get; set; } = [];
        public bool IsPinned { get; set; }
    }
}
