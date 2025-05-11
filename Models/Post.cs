using YssWebstoreApi.Models.Interfaces;

namespace YssWebstoreApi.Models
{
    public class Post : IEntity
    {
        public Guid Id { get; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid AccountId { get; set; }
        public Guid? ProjectId { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public Image? Image { get; set; }
    }
}
