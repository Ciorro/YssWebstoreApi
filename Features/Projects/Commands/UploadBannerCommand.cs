using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Commands
{
    public class UploadBannerCommand : ICommand<ValueResult<string>>
    {
        public Guid AccountId { get; }
        public Guid ProjectId { get; }
        public IFormFile Banner { get; }

        public UploadBannerCommand(Guid accountId, Guid projectId, IFormFile banner)
        {
            AccountId = accountId;
            ProjectId = projectId;
            Banner = banner;
        }
    }
}
