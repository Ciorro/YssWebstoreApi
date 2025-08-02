using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Commands
{
    public class DeleteIconCommand : ICommand<Result>
    {
        public Guid AccountId { get; }
        public Guid ProjectId { get; }

        public DeleteIconCommand(Guid accountId, Guid projectId)
        {
            AccountId = accountId;
            ProjectId = projectId;
        }
    }
}
