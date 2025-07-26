using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Commands
{
    public class CreateProjectReviewCommand : ICommand<Result<Guid>>
    {
        public Guid AccountId { get; }
        public Guid ProjectId { get; }
        public int Rate { get; }
        public string? Content { get; init; }

        public CreateProjectReviewCommand(Guid accountId, Guid projectId, int rate)
        {
            AccountId = accountId;
            ProjectId = projectId;
            Rate = rate;
        }
    }
}
