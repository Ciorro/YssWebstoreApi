using YssWebstoreApi.Entities.Interfaces;

namespace YssWebstoreApi.Entities
{
    public class Package : Resource, IEntity
    {
        public required string Name { get; set; }
        public required string Version { get; set; }
        public OperatingSystem TargetOS { get; set; }
        public long Size { get; set; }
    }
}
