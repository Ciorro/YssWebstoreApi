using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Accounts.Commands
{
    internal class UpdateAccountCommand : ICommand<Result>
    {
        public Guid AccountId { get; }
        public string DisplayName { get; }
        public string? StatusText { get; set; }

        public UpdateAccountCommand(Guid accountId, string? displayName)
        {
            AccountId = accountId;
            DisplayName = displayName;
        }
    }
}
