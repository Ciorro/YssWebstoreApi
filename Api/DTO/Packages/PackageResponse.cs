namespace YssWebstoreApi.Api.DTO.Packages
{
    public class PackageResponse
    {
        public required Guid Id { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required DateTime UpdatedAt { get; set; }
        public required string Name { get; set; }
        public required string Version { get; set; }
        public required long Size { get; set; }
        public Entities.OperatingSystem TargetOS { get; set; }
    }
}
