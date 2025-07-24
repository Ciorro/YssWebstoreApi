using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Posts.Commands
{
    public class SetProjectPinnedCommand : ICommand<Result>
    {
        public Guid AccountId { get; set; }
        public Guid ProjectId { get; set; }
        public bool IsPinned { get; set; }

        public SetProjectPinnedCommand(Guid accountId, Guid projectId, bool isPinned)
        {
            AccountId = accountId;
            ProjectId = projectId;
            IsPinned = isPinned;
        }
    }
}
