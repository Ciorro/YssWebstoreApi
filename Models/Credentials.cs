using YssWebstoreApi.Models.Abstractions;

namespace YssWebstoreApi.Models
{
    public class Credentials : IEntity
    {
        public ulong? Id { get; set; }
        public ulong? AccountId { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? PasswordSalt { get; set; }
        public string? RefreshToken { get; set; }
        public string? VerificationCode { get; set; }
        public string? PasswordResetCode { get; set; }
        public DateTimeOffset? RefreshTokenExpiresAt { get; set; }
        public DateTimeOffset? VerificationCodeExpiresAt { get; set; }
        public DateTimeOffset? PasswordResetCodeExpiresAt { get; set; }
        public bool IsVerified { get; set; }
    }
}
