using MediatR;
using System.Diagnostics.CodeAnalysis;
using YssWebstoreApi.Models.DTOs.Auth;

namespace YssWebstoreApi.Features.Queries.Auth
{
    public class TokenSignInQuery : IRequest<TokenCredentials?>
    {
        public required ulong AccountId { get; init; }
        public required string RefreshToken { get; init; }

        public TokenSignInQuery() { }

        [SetsRequiredMembers]
        public TokenSignInQuery(ulong accountId, string refreshToken)
        {
            AccountId = accountId;
            RefreshToken = refreshToken;
        }
    }
}
