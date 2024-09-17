namespace YssWebstoreApi.Models.DTOs.Auth
{
    public class SignUpCredentials
    {
        public required string Email { get; init; }
        public required string Password { get; init; }
    }
}
