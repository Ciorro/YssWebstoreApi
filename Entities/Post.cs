using YssWebstoreApi.Entities.Interfaces;

namespace YssWebstoreApi.Entities
{
    public class Post : IEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid AccountId { get; set; }
        public Resource? Image { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public Guid? TargetProjectId { get; set; }
    }
}
