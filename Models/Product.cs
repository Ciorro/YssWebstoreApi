using YssWebstoreApi.Models.Abstractions;

namespace YssWebstoreApi.Models
{
    public class Product : IEntity
    {
        public ulong? Id { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public ulong? AccountId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? SourceUrl { get; set; }
        public bool? IsPinned { get; set; }
    }
}
