namespace YssWebstoreApi.Api.DTO.Packages
{
    public class PackageResponse
    {
        public required string Name { get; set; }
        public required string Version { get; set; }
        public required long Size { get; set; }
        public Entities.OperatingSystem TargetOS { get; set; }
    }
}
