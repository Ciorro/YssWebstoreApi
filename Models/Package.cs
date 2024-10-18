namespace YssWebstoreApi.Models
{
    public class Package
    {
        public ulong? Id { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public ulong? ProductId { get; set; } 
        public ulong? FileSize { get; set; }
        public string? Name { get; set; }
        public string? Version { get; set; }
        public string? DownloadUrl { get; set; }
        public OS? TargetOs { get; set; }
    }
}
