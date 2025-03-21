namespace YssWebstoreApi.Models.DTOs.Auth
{
    public class SignInCredentials
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
