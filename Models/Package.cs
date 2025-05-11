using YssWebstoreApi.Models.Interfaces;

namespace YssWebstoreApi.Models
{
    public class Package : IEntity
    {
        public Guid Id { get; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public required string Name { get; set; }
        public required string Version { get; set; }
        public OperatingSystem TargetOS { get; set; }
        public ulong DownloadSize { get; set; }
    }
}
