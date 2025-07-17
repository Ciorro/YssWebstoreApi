using YssWebstoreApi.Models.Interfaces;

namespace YssWebstoreApi.Models
{
    public class Review : IEntity
    {
        public Guid Id { get; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid AccountId { get; set; }
        public Guid ProjectId { get; set; }
        public int Rate { get; set; }
        public string? Content { get; set; }
    }
}
