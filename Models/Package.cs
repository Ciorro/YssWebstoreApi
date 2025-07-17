using YssWebstoreApi.Models.Interfaces;

namespace YssWebstoreApi.Models
{
    public class Package : Resource, IEntity
    {
        public required string Name { get; set; }
        public required string Version { get; set; }
        public OperatingSystem TargetOS { get; set; }
    }
}
