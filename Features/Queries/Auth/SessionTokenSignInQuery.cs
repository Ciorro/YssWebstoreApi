using MediatR;
using System.Diagnostics.CodeAnalysis;
using YssWebstoreApi.Models;

namespace YssWebstoreApi.Features.Queries.Auth
{
    public class SessionTokenSignInQuery : IRequest<Account?>
    {
        public required ulong AccountId { get; init; }
        public required string SessionToken { get; init; }

        public SessionTokenSignInQuery() { }

        [SetsRequiredMembers]
        public SessionTokenSignInQuery(ulong accountId, string sessionToken)
        {
            AccountId = accountId;
            SessionToken = sessionToken;
        }
    }
}
