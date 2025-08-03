using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Posts.Commands
{
    public class UploadCoverImageCommand : ICommand<Result>
    {
        public Guid AccountId { get; }
        public Guid PostId { get; }
        public IFormFile File { get; }

        public UploadCoverImageCommand(Guid accountId, Guid postId, IFormFile file)
        {
            AccountId = accountId;
            PostId = postId;
            File = file;
        }
    }
}
