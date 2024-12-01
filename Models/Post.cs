using YssWebstoreApi.Models.Abstractions;

namespace YssWebstoreApi.Models
{
    public class Post : IEntity
    {
        public ulong? Id { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public ulong? ImageId { get; set; }
        public ulong? AccountId { get; set; }
        public ulong? ProductId { get; set; }
    }
}
