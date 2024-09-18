namespace YssWebstoreApi.Models.DTOs.Package
{
    public class CreatePackage
    {
        public required uint ProductId { get; set; }
        public required string Name { get; set; }
        public required string Version { get; set; }
        public required string DownloadUrl { get; set; }
        public OS? TargetOS { get; set; }
    }
}
