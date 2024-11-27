using YssWebstoreApi.Models.Abstractions;

namespace YssWebstoreApi.Models
{
    public class Review : IEntity
    {
        public ulong? Id { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public ulong? AccountId { get; set; }
        public ulong? ProductId { get; set; }
        public int? Rate { get; set; }
        public string? Content { get; set; }
    }
}
