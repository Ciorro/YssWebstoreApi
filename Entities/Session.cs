using YssWebstoreApi.Entities.Interfaces;

namespace YssWebstoreApi.Entities
{
    public class Session : IEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public required string SessionToken { get; set; }
        public string? DeviceInfo { get; set; }
    }
}
