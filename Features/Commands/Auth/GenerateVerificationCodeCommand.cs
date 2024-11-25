using MediatR;

namespace YssWebstoreApi.Features.Commands.Auth
{
    public class GenerateVerificationCodeCommand(ulong accountId) : IRequest<bool>
    {
        public ulong AccountId { get; } = accountId;
    }
}
