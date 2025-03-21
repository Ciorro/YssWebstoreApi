using YssWebstoreApi.Models.Abstractions;

namespace YssWebstoreApi.Models
{
    public class Session : IEntity
    {
        public ulong? Id { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public DateTimeOffset? ExpiresAt { get; set; }
        public ulong? AccountId { get; set; }
        public string? SessionToken { get; set; }
    }
}
