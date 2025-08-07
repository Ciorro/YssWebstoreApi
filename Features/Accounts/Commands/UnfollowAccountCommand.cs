using LiteBus.Commands.Abstractions;
using YssWebstoreApi.Utils;

namespace YssWebstoreApi.Features.Accounts.Commands
{
    public class UnfollowAccountCommand : ICommand<Result>
    {
        public Guid FollowerId { get; }
        public Guid FollowedId { get; }

        public UnfollowAccountCommand(Guid followerId, Guid followedId)
        {
            FollowerId = followerId;
            FollowedId = followedId;
        }
    }
}
