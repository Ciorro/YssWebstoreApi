using YssWebstoreApi.Models.Interfaces;

namespace YssWebstoreApi.Models
{
    public class Credentials : IEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public required string PasswordSalt { get; set; }
        public string? VerificationCode { get; set; }
        public string? PasswordResetCode { get; set; }
        public DateTime? VerificationCodeExpiresAt { get; set; }
        public DateTime? PasswordResetCodeExpiresAt { get; set; }
        public bool IsVerified { get; set; }
    }
}
