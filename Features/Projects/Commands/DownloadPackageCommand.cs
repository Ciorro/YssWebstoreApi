using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Commands
{
    public class DownloadPackageCommand : ICommand<ValueResult<string>>
    {
        public Guid? AccountId { get; set; }
        public Guid ProjectId { get; set; }
        public Guid PackageId { get; set; }

        public DownloadPackageCommand(Guid projectId, Guid packageId)
        {
            ProjectId = projectId;
            PackageId = packageId;
        }
    }
}
