using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Accounts.Commands
{
    public class CreateVerificationCodeCommand(Guid accountId) : ICommand<ValueResult<string>>
    {
        public Guid AccountId { get; } = accountId;
    }
}
