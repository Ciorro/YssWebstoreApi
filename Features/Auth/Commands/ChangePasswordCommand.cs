using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Auth.Commands
{
    public class ChangePasswordCommand : ICommand<Result>
    {
        public Guid AccountId { get; }
        public string OldPassword { get; }
        public string NewPassword { get; }

        public ChangePasswordCommand(Guid accountId, string oldPassword, string newPassword)
        {
            AccountId = accountId;
            OldPassword = oldPassword;
            NewPassword = newPassword;
        }
    }
}
