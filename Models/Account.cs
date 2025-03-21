using YssWebstoreApi.Models.Abstractions;

namespace YssWebstoreApi.Models
{
    public class Account : IEntity
    {
        public ulong? Id { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public string? UniqueName { get; set; }
        public string? DisplayName { get; set; }
        public string? Status { get; set; }
        public bool IsVerified { get; set; }
    }
}
