using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Accounts.Commands
{
    public class DeleteAvatarCommand(Guid accountId) : ICommand<Result>
    {
        public Guid AccountId { get; } = accountId;
    }
}
