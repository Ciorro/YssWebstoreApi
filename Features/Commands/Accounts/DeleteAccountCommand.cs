using MediatR;

namespace YssWebstoreApi.Features.Commands.Accounts
{
    public class DeleteAccountCommand(ulong accountId) : IRequest<bool>
    {
        public ulong AccountId { get; } = accountId;
    }
}
