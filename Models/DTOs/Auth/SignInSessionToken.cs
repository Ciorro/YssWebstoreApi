namespace YssWebstoreApi.Models.DTOs.Auth
{
    public class SignInSessionToken
    {
        public required Guid AccountId { get; set; }
        public required string SessionToken { get; set; }
    }
}
