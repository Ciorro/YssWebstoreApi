using MediatR;
using System.Diagnostics.CodeAnalysis;
using YssWebstoreApi.Models;

namespace YssWebstoreApi.Features.Commands.Auth
{
    public class ExtendSessionCommand : IRequest<Session?>
    {
        public required Account Account { get; init; }
        public required string SessionToken { get; init; }

        public ExtendSessionCommand() { }

        [SetsRequiredMembers]
        public ExtendSessionCommand(Account account, string existingSessionToken)
        {
            Account = account;
            SessionToken = existingSessionToken;
        }
    }
}
