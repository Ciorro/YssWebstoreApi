namespace YssWebstoreApi.Models.DTOs.Package
{
    public class PublicPackage
    {
        public required ulong Id { get; set; }
        public required DateTimeOffset CreatedAt { get; set; }
        public required DateTimeOffset UpdatedAt { get; set; }
        public required ulong ProductId { get; set; }
        public required ulong FileSize { get; set; }
        public required string Name { get; set; }
        public required string Version { get; set; }
        public required string DownloadUrl { get; set; }
        public OS? TargetOS { get; set; }
    }
}
