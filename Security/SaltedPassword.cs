using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;

namespace YssWebstoreApi.Security
{
    public record SaltedPassword
    {
        public required string PasswordHash { get; init; }
        public required string PasswordSalt { get; init; }

        public SaltedPassword() { }

        [SetsRequiredMembers]
        public SaltedPassword(string password)
        {
            var newPassword = Create(password, SecurityUtils.GetRandomString(32));
            PasswordHash = newPassword.PasswordHash;
            PasswordSalt = newPassword.PasswordSalt;
        }

        public static SaltedPassword Create(string password, string passwordSalt)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(password, nameof(password));
            ArgumentException.ThrowIfNullOrWhiteSpace(passwordSalt, nameof(passwordSalt));

            string passwordHash = password + passwordSalt;
            var passwordData = Encoding.UTF8.GetBytes(password);

            using (var sha256 = SHA256.Create())
            {
                passwordData = sha256.ComputeHash(passwordData);
            }

            passwordHash = Convert.ToBase64String(passwordData);

            return new SaltedPassword
            {
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };
        }
    }
}
