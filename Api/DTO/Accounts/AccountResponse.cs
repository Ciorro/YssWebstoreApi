namespace YssWebstoreApi.Api.DTO.Accounts
{
    public class AccountResponse
    {
        public required string UniqueName { get; set; }
        public required string DisplayName { get; set; }
        public string? StatusText { get; set; }
    }
}
