namespace YssWebstoreApi.Models.DTOs.Accounts
{
    public class PrivateAccount
    {
        public required uint Id { get; set; }
        public required DateTimeOffset CreatedAt { get; set; }
        public required DateTimeOffset UpdatedAt { get; set; }
        public required string Email {  get; set; }
        public required string UniqueName { get; set; }
        public required string DisplayName { get; set; }
        public string Status { get; set; } = "";
    }
}
