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
            : this(password, SecurityUtils.GetRandomString(32))
        { }

        [SetsRequiredMembers]
        public SaltedPassword(string password, string passwordSalt)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(password, nameof(password));
            ArgumentException.ThrowIfNullOrWhiteSpace(passwordSalt, nameof(passwordSalt));

            PasswordSalt = passwordSalt;
            PasswordHash = password + PasswordSalt;

            var passwordData = Encoding.UTF8.GetBytes(password);

            using (var sha256 = SHA256.Create())
            {
                passwordData = sha256.ComputeHash(passwordData);
            }

            PasswordHash = Convert.ToBase64String(passwordData);
        }
    }
}
