using YssWebstoreApi.Entities.Interfaces;

namespace YssWebstoreApi.Entities
{
    public class Review : IEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid AccountId { get; set; }
        public Guid ProjectId { get; set; }
        public int Rate { get; set; }
        public string? Content { get; set; }
    }
}
