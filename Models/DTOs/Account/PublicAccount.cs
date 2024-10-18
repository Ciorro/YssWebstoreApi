namespace YssWebstoreApi.Models.DTOs.Accounts
{
    public class PublicAccount
    {
        public required ulong Id { get; set; }
        public required DateTimeOffset CreatedAt { get; set; }
        public required string UniqueName { get; set; }
        public required string DisplayName { get; set; }
        public string Status { get; set; } = "";
    }
}
