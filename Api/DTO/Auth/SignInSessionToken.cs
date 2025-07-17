namespace YssWebstoreApi.Api.DTO.Auth
{
    public class SignInSessionToken
    {
        public required Guid AccountId { get; set; }
        public required string SessionToken { get; set; }
    }
}
