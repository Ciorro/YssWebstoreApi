using MediatR;

namespace YssWebstoreApi.Features.Queries.Auth
{
    public class GetValidRefreshTokenQuery(ulong accountId) : IRequest<string?>
    {
        public ulong AccountId { get; } = accountId;
    }
}
