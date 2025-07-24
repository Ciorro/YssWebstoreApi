using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Posts.Commands
{
    public class AttachImageToPostCommand : ICommand<Result>
    {
        public Guid AccountId { get; }
        public Guid PostId { get; }
        public IFormFile File { get; }

        public AttachImageToPostCommand(Guid accountId, Guid postId, IFormFile file)
        {
            AccountId = accountId;
            PostId = postId;
            File = file;
        }
    }
}
