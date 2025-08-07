using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Accounts.Commands
{
    public class FollowAccountCommand : ICommand<Result>
    {
        public Guid FollowerId { get; }
        public Guid FollowedId { get; }

        public FollowAccountCommand(Guid followerId, Guid followedId)
        {
            FollowerId = followerId;
            FollowedId = followedId;
        }
    }
}
