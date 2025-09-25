using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Posts.Commands
{
    public class DeletePostCommand : ICommand<Result>
    {
        public Guid AccountId { get; set; }
        public Guid PostId { get; set; }

        public DeletePostCommand(Guid accountId, Guid postId)
        {
            AccountId = accountId;
            PostId = postId;
        }
    }
}
