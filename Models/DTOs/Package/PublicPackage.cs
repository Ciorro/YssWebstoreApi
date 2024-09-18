namespace YssWebstoreApi.Models.DTOs.Package
{
    public class PublicPackage
    {
        public required uint Id { get; set; }
        public required DateTimeOffset CreatedAt { get; set; }
        public required DateTimeOffset UpdatedAt { get; set; }
        public required uint ProductId { get; set; }
        public required string Name { get; set; }
        public required string Version { get; set; }
        public required string DownloadUrl { get; set; }
        public OS? TargetOS { get; set; }
    }
}
