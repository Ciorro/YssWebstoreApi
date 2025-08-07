using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace YssWebstoreApi.Features.Commands.Auth
{
    public class GetProlongedRefreshTokenCommand : IRequest<string?>
    {
        public required ulong AccountId { get; init; }
        public string? CurrentRefreshToken { get; init; }

        public GetProlongedRefreshTokenCommand() { }

        [SetsRequiredMembers]
        public GetProlongedRefreshTokenCommand(ulong accountId)
        {
            AccountId = accountId;
        }
    }
}
