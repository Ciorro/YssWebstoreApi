using YssWebstoreApi.Models.Interfaces;

namespace YssWebstoreApi.Models
{
    public class Account : IEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public required string UniqueName { get; set; }
        public required string DisplayName { get; set; }
        public required Credentials Credentials { get; set; }
        public ICollection<Session> Sessions { get; set; } = [];
        public string? StatusText { get; set; }
    }
}
