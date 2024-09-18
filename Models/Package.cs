namespace YssWebstoreApi.Models
{
    public class Package
    {
        public uint? Id { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public uint? ProductId { get; set; }
        public string? Name { get; set; }
        public string? Version { get; set; }
        public string? DownloadUrl { get; set; }
        public OS? TargetOS { get; set; }
    }
}
