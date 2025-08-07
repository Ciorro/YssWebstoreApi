using System.ComponentModel.DataAnnotations;

namespace YssWebstoreApi.Api.DTO.Packages
{
    public class UploadPackageRequest
    {
        [Length(3, 80)]
        public required string Name { get; set; }
        [Length(1, 32)]
        public required string Version { get; set; }
        public required IFormFile File { get; set; }
        public Entities.OperatingSystem TargetOS { get; set; }
    }
}
