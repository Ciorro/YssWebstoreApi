namespace YssWebstoreApi.Api.DTO.Auth
{
    public class TokenCredentials
    {
        public required string AccessToken { get; set; }
        public required string SessionToken { get; set; }
    }
}
