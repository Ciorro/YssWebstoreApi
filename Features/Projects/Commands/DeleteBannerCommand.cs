using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Commands
{
    public class DeleteBannerCommand : ICommand<Result>
    {
        public Guid AccountId { get; }
        public Guid ProjectId { get; }

        public DeleteBannerCommand(Guid accountId, Guid projectId)
        {
            AccountId = accountId;
            ProjectId = projectId;
        }
    }
}
