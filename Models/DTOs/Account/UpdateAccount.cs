namespace YssWebstoreApi.Models.DTOs.Accounts
{
    public class UpdateAccount
    {
        public required string UniqueName { get; set; }
        public required string DisplayName { get; set; }
        public string? Status { get; set; }
    }
}
