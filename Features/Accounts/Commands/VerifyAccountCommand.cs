using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Accounts.Commands
{
    public class VerifyAccountCommand : ICommand<Result>
    {
        public Guid AccountId { get; }
        public string VerificationCode { get; }

        public VerifyAccountCommand(Guid accountId, string verificationCode)
        {
            AccountId = accountId;
            VerificationCode = verificationCode;
        }
    }
}
