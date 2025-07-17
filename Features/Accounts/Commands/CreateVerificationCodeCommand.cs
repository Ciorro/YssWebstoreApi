using LiteBus.Commands.Abstractions;

namespace YssWebstoreApi.Features.Accounts.Commands
{
    public class CreateVerificationCodeCommand(Guid accountId) : ICommand<Result<string>>
    {
        public Guid AccountId { get; } = accountId;
    }
}
