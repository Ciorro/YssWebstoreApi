using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Commands
{
    public class DeleteReviewCommand : ICommand<Result>
    {
        public Guid AccountId { get; }
        public Guid ProjectId { get; }

        public DeleteReviewCommand(Guid accountId, Guid projectId)
        {
            AccountId = accountId;
            ProjectId = projectId;
        }
    }
}
