namespace YssWebstoreApi.Models.DTOs.Auth
{
    public class TokenCredentials
    {
        public required string AccessToken { get; set; }
        public required string SessionToken { get; set; }
    }
}
