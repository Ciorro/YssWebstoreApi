namespace YssWebstoreApi.Api.DTO.Accounts
{
    public class UpdateAccountRequest
    {
        public required string DisplayName { get; set; }
        public string? StatusText { get; set; }
    }
}
