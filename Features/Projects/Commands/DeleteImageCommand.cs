using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Commands
{
    public class DeleteImageCommand : ICommand<Result>
    {
        public Guid AccountId { get; }
        public Guid ProjectId { get; }
        public Guid ImageId { get; }

        public DeleteImageCommand(Guid accountId, Guid projectId, Guid imageId)
        {
            AccountId = accountId;
            ProjectId = projectId;
            ImageId = imageId;
        }
    }
}
