namespace YssWebstoreApi.Api.DTO.Packages
{
    public class UploadPackageRequest
    {
        public required string Name { get; set; }
        public required string Version { get; set; }
        public required IFormFile File { get; set; }
        public Entities.OperatingSystem TargetOS { get; set; }
    }
}
