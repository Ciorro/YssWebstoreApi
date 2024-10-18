using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace YssWebstoreApi.Features.Commands.Auth
{
    public class GetProlongedRefreshTokenCommand : IRequest<string?>
    {
        public required ulong AccountId { get; init; }
        public required TimeSpan Length { get; init; }
        public string? CurrentRefreshToken { get; init; }

        public GetProlongedRefreshTokenCommand() { }

        [SetsRequiredMembers]
        public GetProlongedRefreshTokenCommand(ulong accountId)
            : this(accountId, TimeSpan.FromDays(7))
        { }

        [SetsRequiredMembers]
        public GetProlongedRefreshTokenCommand(ulong accountId, TimeSpan length)
        {
            AccountId = accountId;
            Length = length;
        }
    }
}
