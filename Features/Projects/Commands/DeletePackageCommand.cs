using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Commands
{
    public class DeletePackageCommand : ICommand<Result>
    {
        public Guid AccountId { get; }
        public Guid ProjectId { get; }
        public Guid PackageId { get; }

        public DeletePackageCommand(Guid accountId, Guid projectId, Guid packageId)
        {
            AccountId = accountId;
            ProjectId = projectId;
            PackageId = packageId;
        }
    }
}
