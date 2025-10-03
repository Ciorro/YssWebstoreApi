using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Posts.Commands
{
    public class UpdatePostCommand : ICommand<Result>
    {
        public Guid AccountId { get; set; }
        public Guid PostId { get; set; }
        public string Title { get; }
        public string Content { get; }
        public Guid? TargetProjectId { get; init; }

        public UpdatePostCommand(Guid accountId, Guid postId, string title, string content)
        {
            AccountId = accountId;
            PostId = postId;
            Title = title;
            Content = content;
        }
    }
}
