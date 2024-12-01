using YssWebstoreApi.Models.Abstractions;

namespace YssWebstoreApi.Models
{
    public class Image : IEntity, IAttachment
    {
        public ulong? Id { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public string? Title { get; set; }
        public string? Path { get; set; }
        public ulong? AccountId { get; set; }
    }
}
