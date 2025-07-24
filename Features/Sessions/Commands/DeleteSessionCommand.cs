using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Sessions.Commands
{
    public class DeleteSessionCommand : ICommand<Result>
    {
        public Guid AccountId { get; }
        public string SessionToken { get; }

        public DeleteSessionCommand(Guid accountId, string sessionToken)
        {
            AccountId = accountId;
            SessionToken = sessionToken;
        }
    }
}
