using System.Diagnostics.CodeAnalysis;

namespace YssWebstoreApi.Models
{
    public class RefreshToken
    {
        public required string Token { get; init; }
        public required DateTimeOffset Expires { get; init; }

        public RefreshToken() { }

        [SetsRequiredMembers]
        public RefreshToken(string token, DateTimeOffset expires)
        {
            Token = token;
            Expires = expires;
        }
    }
}
