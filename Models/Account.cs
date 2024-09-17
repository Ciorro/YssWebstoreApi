namespace YssWebstoreApi.Models
{
    public class Account
    {
        public uint? Id { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public string? UniqueName { get; set; }
        public string? DisplayName { get; set; }
    }
}
