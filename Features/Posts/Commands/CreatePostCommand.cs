using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Posts.Commands
{
    public class CreatePostCommand : ICommand<Result<Guid>>
    {
        public Guid AccountId { get; }
        public string Title { get; }
        public string Content { get; }
        public Guid? ImageResourceId { get; init; }
        public Guid? TargetProjectId { get; init; }

        public CreatePostCommand(Guid accountId, string title, string content)
        {
            AccountId = accountId;
            Title = title;
            Content = content;
        }
    }
}
