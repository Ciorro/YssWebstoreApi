using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Projects.Commands
{
    public class ReorderImageCommand : ICommand<Result>
    {
        public Guid AccountId { get; }
        public Guid ProjectId { get; }
        public Guid ImageId { get; }
        public int NewOrder { get; }

        public ReorderImageCommand(Guid accountId, Guid projectId, Guid imageId, int newOrder)
        {
            AccountId = accountId;
            ProjectId = projectId;
            ImageId = imageId;
            NewOrder = newOrder;
        }
    }
}
