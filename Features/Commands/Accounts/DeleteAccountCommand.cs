using MediatR;

namespace YssWebstoreApi.Features.Commands.Accounts
{
    public class DeleteAccountCommand(ulong accountId) : IRequest<ulong?>
    {
        public ulong AccountId { get; } = accountId;
    }
}
