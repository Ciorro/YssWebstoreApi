using LiteBus.Commands.Abstractions;

namespace YssWebstoreApi.Features.Sessions.Commands
{
    public class DeleteAllSessionsCommand(Guid accountId) : ICommand<Result>
    {
        public Guid AccountId { get; } = accountId;
    }
}
