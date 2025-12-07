using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Api.DTO.Auth;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Sessions.Commands
{
    public class UpdateSessionCommand : ICommand<ValueResult<TokenCredentials>>
    {
        public Guid AccountId { get; }
        public string SessionToken { get; }

        public UpdateSessionCommand(Guid accountId, string sessionToken)
        {
            AccountId = accountId;
            SessionToken = sessionToken;
        }
    }
}
