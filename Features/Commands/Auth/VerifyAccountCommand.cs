using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace YssWebstoreApi.Features.Commands.Auth
{
    public class VerifyAccountCommand : IRequest<bool>
    {
        public required ulong AccountId {  get; set; }
        public required string VerificationCode { get; set; }

        public VerifyAccountCommand() { }

        [SetsRequiredMembers]
        public VerifyAccountCommand(ulong accountId, string verificationCode)
        {
            AccountId = accountId;
            VerificationCode = verificationCode;
        }
    }
}
