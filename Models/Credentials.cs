namespace YssWebstoreApi.Models
{
    public class Credentials
    {
        public uint? Id { get; set; }
        public uint? AccountId { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? PasswordSalt { get; set; }
        public string? RefreshToken { get; set; }
        public string? VerificationCode { get; set; }
        public DateTimeOffset? RefreshTokenExpiresAt { get; set; }
        public DateTimeOffset? VerificationCodeExpiresAt { get; set; }
        public bool IsVerified { get; set; }
    }
}
