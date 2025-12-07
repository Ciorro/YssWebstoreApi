using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Accounts.Commands
{
    public class UploadAvatarCommand : ICommand<ValueResult<string>>
    {
        public Guid AccountId { get; }
        public IFormFile File { get; }

        public UploadAvatarCommand(Guid accountId, IFormFile file)
        {
            AccountId = accountId;
            File = file;
        }
    }
}
