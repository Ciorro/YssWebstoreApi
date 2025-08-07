using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Posts.Commands
{
    public class RemoveImageFromPostCommand : ICommand<Result>
    {
        public Guid AccountId { get; }
        public Guid PostId { get; }

        public RemoveImageFromPostCommand(Guid accountId, Guid postId)
        {
            AccountId = accountId;
            PostId = postId;
        }
    }
}
